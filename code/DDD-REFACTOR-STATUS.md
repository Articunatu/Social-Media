# DDD Refactor — Status & Handoff

> Companion to [DDD-REFACTORING-GUIDE.md](DDD-REFACTORING-GUIDE.md). This file is the source of truth for **what is done** and **what to do next**. Read it fully before making changes.

- **Guide:** [DDD-REFACTORING-GUIDE.md](DDD-REFACTORING-GUIDE.md) (target architecture + migration order §14)
- **Backend root:** `code/server/SocialMedia/`
- **Solution:** `SocialMedia.sln` (projects target `net9.0`; SDK 10.0.300 installed)

---

## How to work on this refactor

**Ground rules (follow these exactly):**

1. **Incremental only.** One phase (or slice) at a time. Do **not** do a big-bang rewrite or vertical-slice rewrite.
2. **No DB migration unless unavoidable.** Keep EF FK column names, cascade behaviors, and table/column names identical so no schema migration is required. Use `HasColumnName(...)` / `ToTable(...)` / shadow FKs to preserve the existing schema.
3. **Keep the build green and all tests passing after every step.** Never move to the next phase with a red build or failing tests.
4. **Grouping stays CQRS-by-entity.** Commands/Queries/Handlers are grouped by entity folder. Do not restructure into vertical slices.
5. **Pause at checkpoints.** After finishing a phase, report briefly (with file links) and ask before starting the next phase.
6. **Don't create markdown docs** unless explicitly asked. Update *this* file's checklist instead.

**Build:**
```powershell
cd C:\Users\ChristofferBrandt\Documents\GitHub\Social-Media\code\server\SocialMedia
dotnet build SocialMedia.sln -v q -clp:ErrorsOnly
```

**Test (run each project separately):**
```powershell
dotnet test SM.Application.IntegrationTests/SM.Application.IntegrationTests.csproj --nologo -v q
dotnet test SM.Application.UnitTests/SM.Application.UnitTests.csproj --nologo -v q
dotnet test SM.Domain.UnitTests/SM.Domain.UnitTests.csproj --nologo -v q
```

**Green baseline:** 14 integration + 34 application unit + 3 domain unit tests — all passing.

**Environment notes:**
- Windows PowerShell — chain commands with `;`, not `&&`.
- Integration tests use the EF Core **InMemory** provider (`IntegrationTestFixture`). Production uses `UseSqlServer`.
- `NU1900` warnings (TenstarAB feed) are benign / pre-existing — ignore.
- EF `ModelSnapshot` has known/accepted drift (still references old `SM.Domain.Messages.*` type strings). Runtime maps by CLR type + `ToTable`/`HasColumnName`, so it works. Regenerate the snapshot on the next `dotnet ef migrations add`.

---

## Target bounded contexts

`Identity` · `Content` · `Messaging` · `SocialGraph` · `Media` · `Notifications`

The `User` God Entity has been fully decomposed. Other contexts reference `User` **only by `Guid`** (no navigation properties into Identity).

---

## Progress checklist (migration order — guide §14)

| # | Phase | Status |
|---|-------|--------|
| 1 | Add tests | ✅ Pre-existing |
| 2 | Application / CQRS layer | ✅ Done |
| 3 | Extract Content (`SM.Domain.Content`) | ✅ Done |
| 3b| Remove navigation abuse (no Content→Identity navs) | ✅ Done |
| 4 | Extract Messaging (`SM.Domain.Messaging`) | ✅ Done (dormant) |
| 5 | Extract Social Graph (`SM.Domain.SocialGraph`) | ✅ Done |
| 6 | **Domain Events expansion** | ✅ Done (in-process) |
| 7 | Split DbContexts | ✅ Done |
| 8 | Split databases (handlers wired to split contexts) | ✅ Done |
| 9–12 | Extract Feed / Messaging / Search / Media services | ⬜ Later (earn the complexity) |

---

## What's completed (detail)

### Phase 2 — Application / CQRS layer ✅
Commands / Queries / Handlers exist, grouped by entity folder under `SM.Application`.

### Phase 3 — Extract Content ✅
- `Post`, `Comment`, `Reaction`, `ReactionType` moved to `SM.Domain.Content`.
- `Content` value object renamed → `ContentText` (`SM.Domain.Content.ValueObjects`).
- Old `SM.Domain/Reactions` folder removed.

### Phase 3b — Remove navigation abuse ✅
Content context has **no** navigation into Identity — only FK `Guid`s.
- Removed `User.AuthoredPosts` / `AuthoredComments` / `Reactions`.
- Removed child-side cross-context navs `Post.Author`, `Comment.Author`, `Reaction.User` (kept `AuthorId` / `UserId` `Guid`s).
- EF configs use `HasOne<User>().WithMany().HasForeignKey(...)` **shadow FKs** — same columns + cascade, no migration.
- Added profile lookup helpers; reaction/comment/feed handlers hydrate author display data via those helpers.

### Phase 4 — Extract Messaging ✅ (dormant)
- New `SM.Domain.Messaging`: `Conversation.cs`, `DirectMessage.cs`, `Events/MessageCreatedDomainEvent.cs`.
- `DirectMessage` inherits `SoftDeletableEntity<Guid>` directly and inlines `Content` / `TimeStamp` / `AuthorId` (no cross-context base sharing).
- Shared base was Content-only afterward → moved to `SM.Domain/Content/AuthoredContent.cs` (renamed from `Message`, `[NotMapped]` abstract). `Post` / `Comment` now `: AuthoredContent(id)`.
- `ApplicationDbContext` does `Ignore<AuthoredContent>()`.
- **Deleted** `SM.Domain/Messages/` folder entirely (`SM.Domain.Messages` namespace is gone).
- ⚠️ Messaging is **dormant**: no EF config, no `DbSet`, no handlers yet. Wire up persistence + commands only if messaging is later required.

### Phase 5 — Extract Social Graph ✅
- New `SM.Domain.SocialGraph.Follow` entity (`FollowerId`, `FollowingId`; private ctor + `Follow.Create` factory).
- Removed `User.Following` / `User.Followers`. **`User` God Entity now holds only `Photos` + auth fields.**
- `FollowConfiguration` maps `Follow` onto the existing `Follows` table with the same schema: `FollowerId`→`FollowersId`, `FollowingId`→`FollowingId`; composite PK; `HasIndex(FollowingId)`; two `HasOne<User>().WithMany()` FKs. No migration required.
- `ApplicationDbContext` temporarily retained for EF migration compatibility and legacy seeding support; runtime code no longer depends on it.
- Rewrote `FollowCommandHandler`, `UnfollowCommandHandler`, `GetProfileQueryHandler`, `GetFeedQueryHandler`, the seeder, and follow integration tests.

### Phase 7 — Split DbContexts ✅
- Added `IdentityDbContext`, `ContentDbContext`, `SocialGraphDbContext`, all inheriting `SocialMediaDbContextBase`.
- Shared `SocialMediaDbContextBase` implements domain event dispatch in `SaveChanges` / `SaveChangesAsync`.
- Registered split contexts in `SM.WebApi.Extensions.ServiceCollectionExtensions` using a shared SQL Server connection string.
- Updated `SM.Application.IntegrationTests.IntegrationTestFixture` to register each split context with InMemory and removed legacy `ApplicationDbContext` registration.
- Rewired handler constructors and queries to use `IDbContextFactory<IdentityDbContext>`, `IDbContextFactory<ContentDbContext>`, and `IDbContextFactory<SocialGraphDbContext>`.
### Phase 6 — Domain Events expansion ✅
- `Post.Create` raises `PostCreatedDomainEvent` with post, author, and creation timestamp.
- `Comment.Create` raises `CommentAddedDomainEvent` with comment, post, author, and creation timestamp.
- `SocialMediaDbContextBase` publishes tracked entity events through MediatR after `SaveChanges` / `SaveChangesAsync`, then clears them.
- In-process application handlers currently log the events; feed, notification, search, analytics, and outbox infrastructure remain future work.
- Added domain tests covering both content event contracts in `SM.Domain.UnitTests/ContentDomainEventTests.cs`.

---

## NEXT: Phase 9 — Extract Feed service

Phase 6 is complete at the current modular-monolith scope. Do not add distributed messaging or an outbox yet; first earn the complexity through a concrete feed projection requirement as described in the guide.

**Goal (guide §6, §9):** replace direct cross-context calls (e.g. `post.Save(); notification.Send();`) with domain events that fan out to Notification / Feed / Search / Analytics, so bounded contexts communicate through events instead of direct coupling (guide Rule 7).

**Current status:**
- split DbContexts and split database wiring are already in place.
- domain event expansion has not yet been implemented.

**Suggested Phase 6 work:**
1. Ensure `Post` and `Comment` aggregates raise domain events such as `PostCreatedEvent`, `CommentAddedEvent`, and/or equivalent activity events.
2. Add event handlers in the relevant bounded contexts, e.g. notifications or feed projection handlers.
3. Verify events are dispatched after `SaveChanges` using `SocialMediaDbContextBase` dispatch logic.
4. Keep work in-process only; do not introduce an outbox or distributed messaging pattern yet.

**Before coding:** grep for the current event pipeline and follow its pattern:
```
IDomainEvent
RaiseDomainEvent
UserCreatedDomainEvent
INotificationHandler
```

**After Phase 6:** update the checklist, keep build + tests green, and pause before starting Phase 9+.

---

## Blockers / Open questions

- Domain event consumers currently log only. Define the first concrete projection contract before adding feed, notification, search, analytics, or outbox infrastructure.
- The messaging context is dormant; confirm whether to keep it as a passive domain model only or to wire real persistence/commands now.
- The runtime seed path is now split to `IdentityDbContext`, `ContentDbContext`, and `SocialGraphDbContext` — verify if any legacy `ApplicationDbContext` test project or preview seed logic still needs cleanup.
- Keep `SM.Application.Migration` / EF model snapshot drift as accepted until a later migration pass rather than regenerating now.
