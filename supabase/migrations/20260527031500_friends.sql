alter table public.profiles
  add column if not exists friend_code text;

update public.profiles
set friend_code = 'NKO-' || upper(substr(replace(id::text, '-', ''), 1, 8))
where friend_code is null;

alter table public.profiles
  alter column friend_code set default ('NKO-' || upper(substr(replace(gen_random_uuid()::text, '-', ''), 1, 8)));

create unique index if not exists profiles_friend_code_key on public.profiles(friend_code);

create table if not exists public.friendships (
  id uuid primary key default gen_random_uuid(),
  user_id uuid not null references auth.users(id) on delete cascade,
  friend_user_id uuid not null references auth.users(id) on delete cascade,
  status text not null default 'accepted',
  created_at timestamptz not null default now(),
  unique (user_id, friend_user_id),
  constraint friendships_no_self check (user_id <> friend_user_id),
  constraint friendships_status_check check (status in ('accepted', 'blocked'))
);

alter table public.friendships enable row level security;

create or replace view public.profile_cards as
select id, display_name, avatar_url, friend_code
from public.profiles;

grant select on public.profile_cards to authenticated;

do $$
begin
  create policy "friendships own read" on public.friendships
    for select using (auth.uid() = user_id or auth.uid() = friend_user_id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "friendships own insert" on public.friendships
    for insert with check (auth.uid() = user_id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "friendships own delete" on public.friendships
    for delete using (auth.uid() = user_id);
exception when duplicate_object then null;
end $$;
