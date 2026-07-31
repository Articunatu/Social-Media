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

**Green baseline:** 14 integration + 34 application unit + 1 domain unit test — all passing.

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
| 6 | **Domain Events expansion** | 🔜 **NEXT** |
| 7 | Split DbContexts | ⬜ Not started |
| 8 | Split databases (only if needed) | ⬜ Not started |
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
- Removed `User.AuthoredPosts` / `AuthoredComments` / `Reactions` (God Entity shrunk).
- Removed child-side cross-context navs `Post.Author`, `Comment.Author`, `Reaction.User` (kept `AuthorId` / `UserId` `Guid`s).
- EF configs use `HasOne<User>().WithMany().HasForeignKey(...)` **shadow FKs** — same columns + cascade, no migration.
- Added profile lookup helpers (see "Key helpers" below); reaction/comment/feed handlers rewritten to hydrate author display data via those helpers.

### Phase 4 — Extract Messaging ✅ (dormant)
- New `SM.Domain.Messaging`: `Conversation.cs`, `DirectMessage.cs`, `Events/MessageCreatedDomainEvent.cs`.
- `DirectMessage` inherits `SoftDeletableEntity<Guid>` directly and inlines `Content` / `TimeStamp` / `AuthorId` (no cross-context base sharing).
- Shared base was Content-only afterward → moved to `SM.Domain/Content/AuthoredContent.cs` (renamed from `Message`, `[NotMapped]` abstract). `Post` / `Comment` now `: AuthoredContent(id)`.
- `ApplicationDbContext` does `Ignore<AuthoredContent>()`.
- **Deleted** `SM.Domain/Messages/` folder entirely (`SM.Domain.Messages` namespace is gone).
- ⚠️ Messaging is **dormant**: no EF config, no `DbSet`, no handlers yet. Wire up persistence + commands (`conversation.SendMessage`) in a later pass if the messaging feature is needed.

### Phase 5 — Extract Social Graph ✅
- New `SM.Domain.SocialGraph.Follow` entity (`FollowerId`, `FollowingId`; private ctor + `Follow.Create` factory). Replaces the `User` self-referencing many-to-many.
- Removed `User.Following` / `User.Followers`. **`User` God Entity now holds only `Photos` + auth fields.**
- `FollowConfiguration` maps `Follow` onto the **existing** `Follows` table: `HasColumnName` `FollowerId`→`"FollowersId"`, `FollowingId`→`"FollowingId"`; composite key `(FollowerId, FollowingId)`; two `HasOne<User>().WithMany()` FKs (`FollowerId` = Cascade, `FollowingId` = ClientCascade); `HasIndex(FollowingId)`. Same schema as the old join table ⇒ **no migration**. Removed the `HasMany(Following).WithMany(Followers).UsingEntity` block from `UserConfiguration`.
- `ApplicationDbContext`: added `DbSet<Follow> Follows`.
- Rewrote `FollowCommandHandler`, `UnfollowCommandHandler`, `GetProfileQueryHandler` (`FollowersCount = Count(f.FollowingId == u.Id)`, `FollowingCount = Count(f.FollowerId == u.Id)`), `GetFeedQueryHandler` (`followingIds` from `Follows.Where(FollowerId == UserId)`), the seeder, and `FollowTests` / `UnfollowTests`.
- **Column semantics:** `FollowersId` = the follower (who follows); `FollowingId` = the followed user.
- ⚠️ **GOTCHA:** the namespace `SM.Application.Users.Follow` (and the sibling test namespace `...IntegrationTests.Users.Follow`) **shadows** the `Follow` type. Fully qualify `SM.Domain.SocialGraph.Follow.Create(...)` in `FollowCommandHandler` and `UnfollowTests`.

---

## Key helpers / conventions (reuse these)

- `SM.Application/Shared/Extensions/UserExtensions.cs`
  - `MapToProfile(User)` → `ProfileInfo(Id, Tag, FullName, ProfilePhoto?)`
  - `MapToCommandResponse(User)` → `UserCommandResponse { Id, Tag, FullName, Email }`
  - `GetProfilePhoto(User)`
- `SM.Application/Shared/Extensions/ProfileQueryExtensions.cs`
  - `GetProfileAsync(ApplicationDbContext, Guid, ct)` → single `ProfileInfo`
  - `GetProfileLookupAsync(ApplicationDbContext, IEnumerable<Guid>, ct)` → `Dictionary<Guid, ProfileInfo>` (Photos loaded)
- Types: `ProfileInfo` → `SM.Application.Shared.Models`; `PagedFeed<T>` → `SM.Application.Abstractions`; `ProfilePostDto` / `ReactionCount` → `SM.Application.Shared.Models`; `FeedResponse` → `SM.Application.Posts.GetFeed`.
- Only System-level implicit usings are global (no domain global usings).

---

## NEXT: Phase 6 — Introduce / expand Domain Events

**Goal (guide §6, §9):** replace direct cross-context calls (e.g. `post.Save(); notification.Send();`) with domain events that fan out to Notification / Feed / Search / Analytics, so bounded contexts communicate through events instead of direct coupling (guide Rule 7).

**Starting point — infra already partially exists:**
- `RaiseDomainEvent` mechanism and `UserCreatedDomainEvent` exist (aggregate base + dispatch).
- `SM.Domain.Messaging.Events.MessageCreatedDomainEvent` exists (record `(Guid Message) : IDomainEvent`) but is not yet raised/handled.

**Suggested Phase 6 work (confirm scope with the user first):**
1. Ensure `Post` (and `Comment`) aggregates raise events: e.g. `PostCreatedEvent`, `CommentAddedEvent` (see guide §9 example `AddDomainEvent(new CommentAddedEvent(PostId, CommentId))`).
2. Add event handlers in the appropriate context (e.g. a Notifications handler reacting to `CommentAddedEvent`).
3. Verify events are dispatched after `SaveChanges` (check the existing dispatch pipeline used by `UserCreatedDomainEvent`).
4. Keep it in-process for now — the **Outbox pattern** (guide §17) and worker-based fan-out are later phases; do not introduce Service Bus / Outbox yet unless asked.

**Before coding:** grep for the existing event dispatch pipeline to match its pattern:
```
IDomainEvent, RaiseDomainEvent, UserCreatedDomainEvent, INotificationHandler
```

**After Phase 6:** update the checklist above, keep build + tests green, then pause and ask before Phase 7 (Split DbContexts).
