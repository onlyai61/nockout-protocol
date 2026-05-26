create extension if not exists pgcrypto;

create table if not exists public.profiles (
  id uuid primary key references auth.users(id) on delete cascade,
  display_name text,
  avatar_url text,
  rc integer not null default 0,
  vc integer not null default 0,
  wc integer not null default 0,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

create table if not exists public.robots (
  id uuid primary key default gen_random_uuid(),
  user_id uuid not null references auth.users(id) on delete cascade,
  name text not null,
  frame_type text not null default 'basic_biped',
  status text not null default 'ready',
  battery_name text,
  battery_capacity integer not null default 100,
  attack integer not null default 1,
  armor integer not null default 1,
  speed integer not null default 1,
  cooling integer not null default 1,
  sensor integer not null default 1,
  power integer not null default 1,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now(),
  constraint robots_stat_bounds check (
    attack between 1 and 99 and
    armor between 1 and 99 and
    speed between 1 and 99 and
    cooling between 1 and 99 and
    sensor between 1 and 99 and
    power between 1 and 99 and
    battery_capacity between 0 and 100
  )
);

create table if not exists public.support_bots (
  id uuid primary key default gen_random_uuid(),
  user_id uuid not null references auth.users(id) on delete cascade,
  name text not null,
  frame_type text not null,
  role text not null,
  level integer not null default 1,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

create table if not exists public.loadouts (
  id uuid primary key default gen_random_uuid(),
  user_id uuid not null references auth.users(id) on delete cascade,
  name text not null,
  combat_robot_id uuid not null references public.robots(id) on delete cascade,
  support_bot_ids uuid[] not null default '{}',
  score integer not null default 0,
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now(),
  unique (user_id, name)
);

create table if not exists public.ranking_entries (
  id uuid primary key default gen_random_uuid(),
  user_id uuid not null references auth.users(id) on delete cascade,
  display_name text,
  loadout_name text,
  score integer not null,
  created_at timestamptz not null default now()
);

create table if not exists public.battle_sessions (
  id uuid primary key default gen_random_uuid(),
  user_id uuid not null references auth.users(id) on delete cascade,
  combat_robot_id uuid not null references public.robots(id) on delete cascade,
  arena_name text not null default 'Practice Ring',
  status text not null default 'active',
  created_at timestamptz not null default now(),
  updated_at timestamptz not null default now()
);

alter table public.profiles enable row level security;
alter table public.robots enable row level security;
alter table public.support_bots enable row level security;
alter table public.loadouts enable row level security;
alter table public.ranking_entries enable row level security;
alter table public.battle_sessions enable row level security;

do $$
begin
  create policy "profiles own read" on public.profiles for select using (auth.uid() = id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "profiles own upsert" on public.profiles for insert with check (auth.uid() = id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "profiles own update" on public.profiles for update using (auth.uid() = id) with check (auth.uid() = id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "robots own all" on public.robots for all using (auth.uid() = user_id) with check (auth.uid() = user_id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "support bots own all" on public.support_bots for all using (auth.uid() = user_id) with check (auth.uid() = user_id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "loadouts own all" on public.loadouts for all using (auth.uid() = user_id) with check (auth.uid() = user_id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "rankings public read" on public.ranking_entries for select using (true);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "rankings own insert" on public.ranking_entries for insert with check (auth.uid() = user_id);
exception when duplicate_object then null;
end $$;

do $$
begin
  create policy "battles own all" on public.battle_sessions for all using (auth.uid() = user_id) with check (auth.uid() = user_id);
exception when duplicate_object then null;
end $$;
