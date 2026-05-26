# Nockout Protocol

ONLY AI 게임 프로젝트 공용 repository입니다.

## 현재 상태

- GitHub repository: https://github.com/onlyai61/nockout-protocol
- Supabase CLI: 사용 준비 완료
- Supabase project link: 프로젝트 ref / access token 필요

## 로컬 세팅

```bash
git clone https://github.com/onlyai61/nockout-protocol.git
cd nockout-protocol
cp .env.example .env.local
```

`.env.local`에는 Supabase Project Settings에서 확인한 값을 넣습니다.
실제 키/비밀번호는 commit하지 않습니다.

Google 로그인 Client ID는 현재 앱 기본값으로 적용되어 있습니다.

```text
956756213946-vsvlk574kejn5oi3klgk7r2jfq8uk45i.apps.googleusercontent.com
```

같은 Client ID와 Client Secret을 Supabase Dashboard의 Authentication Provider > Google에도 저장해야 동작합니다.
Authorized redirect URL은 앱 로그인 화면에 표시되는 값을 Google Cloud Console과 Supabase Redirect URLs에 같이 등록합니다.

## Supabase 연결

Supabase 계정 로그인 후 프로젝트를 연결합니다.

```bash
supabase login
supabase link --project-ref <PROJECT_REF>
```

연결 확인:

```bash
supabase status
supabase projects list
```

## 프로젝트 운영 메모

이 프로젝트는 취미 기반 AI 게임 개발 실험입니다.
부담스러운 업무 분장이 아니라, 각자 가능한 시간에 조금씩 기여하면서 하나의 작품을 만들어가는 방향으로 운영합니다.
