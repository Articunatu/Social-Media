# Agent Guide

This repository has two applications with different structural needs:

- `server/`: .NET backend.
- `client/`: Next.js frontend.

The default framework README is not enough context for agents. Use this guide when adding, moving, or reviewing code.

## General Coding Style

Prefer early returns in both backend and frontend code. Avoid wrapping the main path in `else` blocks when a guard clause can exit first.

Conditional branching inside rendered HTML/JSX is allowed when the condition is actually part of the UI being rendered, such as showing loading, error, empty, or authenticated states. Keep those conditions close to the markup they affect.

## Backend Structure

Keep the backend close to its current shape. It already uses CQRS naming and entity-based folders, which fits the application well.

Preferred backend conventions:

- Group application code by domain/entity, such as `Authentication`, `Posts`, `Comments`, `Users`, `Photos`, and `Reactions`.
- Keep command/query names explicit. A handler should be easy to find from the use case name.
- Do not introduce a vertical-slice rewrite just for symmetry with the client. The backend already gets most of the benefit from entity folders plus CQRS naming.
- Shared abstractions and cross-cutting behavior belong in the existing shared/cross-cutting folders, such as `Abstractions`, `Behaviors`, `Database`, `Shared`, `Extensions`, and `Middlewares`.
- Tests should mirror the backend domain or use case being tested.

## Client Structure

The client should move toward a page-owned, vertical-slice-like structure. Avoid growing broad horizontal folders where every component, hook, validator, and model for every feature sits together.

The guiding rule:

> If code only exists to support one page area, keep it inside that page area's folder.

Use `client/app/pages` as the main feature boundary. A separate top-level `features` folder is not needed.

### Preferred Client Shape

```text
client/app/
  pages/
    authentication/
      login-page.tsx
      register-page.tsx
      forgot-password-page.tsx
      change-password-page.tsx
      components/
      hooks/
      models/
      validation-utils/

    posts/
      feed-page.tsx
      profile-page.tsx
      profile-page-wrapper.tsx
      components/
      hooks/
      models/
      validation-utils/

  components/
    ui/
      react-query-provider.tsx
      sm-button.tsx

  authentication/
    auth-provider.tsx
    logout-button.tsx
    hooks/
    models/

  posts/
    components/
      post-card.tsx
    hooks/
    models/

  services/
    api.ts
    authentication-service.ts
    comment-service.ts
    photo-service.ts
    post-service.ts
    reaction-service.ts
    user-service.ts
    models/
      authentication-models.ts
      comment-models.ts
      post-models.ts
      reaction-models.ts
      user-models.ts

  models/
    paging-models.ts
```

This tree is a target direction, not a requirement to refactor everything at once.

### Page-Owned Code

Put code under `client/app/pages/<page-area>/` when it is owned by that page area:

- Page-only components.
- Hooks that are only used by that page area.
- Validation logic for forms in that page area.
- UI/view models that are not direct API contracts.
- Mapping helpers that prepare data for that page.

Examples:

- Sign-up form validation belongs in `pages/authentication/validation-utils/` unless it becomes shared across authentication flows.
- Feed/profile-only hooks belong in `pages/posts/hooks/` unless they are reused by multiple post-related areas.
- A component used only by `feed-page.tsx` belongs in `pages/posts/components/`.

### File Size And File Boundaries

Prefer one class, type, interface, view, form, hook, or component per file.

Exceptions are allowed when a type is clearly subordinate to the main export in that file, similar to a small nested/sub-class model that is not meaningful on its own. Keep those exceptions small.

Forms should be standalone files even when they are only used in one place. A form usually carries markup, field state, validation messages, submit behavior, and model mapping, so it should not be hidden inside a larger page component.

Keep files at or below 100 lines whenever practical. If a file wants to grow beyond that, split by responsibility before adding more code:

- Move code-behind behavior into a hook.
- Move reusable or bulky markup into a child component.
- Move validation into `validation-utils/`.
- Move form/view models into `models/`.
- Move API mapping into a focused mapper/helper when it is not trivial.

### Shared Entity Folders

Use entity-based folders directly under `client/app` when logic is shared across page areas but still belongs to a domain concept.

Good candidates:

- `client/app/authentication/` for `AuthProvider`, logout behavior, auth session models, and shared authentication hooks.
- `client/app/posts/` for reusable post UI such as `PostCard`, reaction controls, comment controls, and post-specific view models.

Do not make an entity folder just because the backend has one. In the client, entity folders should earn their place by being reused across page areas or routes.

### API Boundary

Keep API services centralized in `client/app/services`.

API request/response models may live with the service boundary because they describe the server contract, not page state. Prefer:

```text
client/app/services/
  post-service.ts
  models/
    post-models.ts
```

If the current `client/app/models/api` folder remains for now, treat it as service-boundary code. Do not mix view models or form models into API model files.

### Models

Use the narrowest meaningful home for models:

- API contracts: `services/models/` or the existing `models/api/` while migrating.
- Page-only form/view models: `pages/<page-area>/models/`.
- Shared entity UI models: `app/<entity>/models/`.
- Truly global utility models, such as paging primitives, may stay in `app/models/`.

Avoid one large `models` folder becoming a junk drawer.

### Hooks

Hooks should sit near their ownership:

- Page-only hooks: `pages/<page-area>/hooks/`.
- Shared entity hooks: `app/<entity>/hooks/`.
- Generic framework hooks with no domain ownership are rare; only then use a shared location.

React Query hooks can still call centralized services. The hook location is about UI ownership, not where HTTP lives.

### Components

Use these component homes:

- `app/components/ui/`: generic UI primitives with no social-media domain knowledge.
- `pages/<page-area>/components/`: components used only by that page area.
- `app/<entity>/components/`: reusable domain components shared across page areas.

Do not put domain components in `app/components` by default. A component like `PostCard` knows about posts, reactions, comments, and auth; it should live under a post-owned folder when moved.

Components should primarily focus on rendering HTML/JSX and wiring user events to callbacks. Keep them easy to scan visually.

Avoid putting heavy code-behind logic directly inside components. Prefer extracting:

- Fetching, mutations, and React Query orchestration into hooks.
- Derived state and event handlers with meaningful branching into hooks.
- Validation into validation utilities.
- Mapping between API models and view models into focused helpers or hooks.

Small local UI state is fine in a component when it is directly tied to rendering, such as whether a small inline panel is open.

### Styling

Use DaisyUI components and utility classes first when they fit the design.

Use Tailwind for specific customization when DaisyUI is not precise enough, such as spacing, layout, borders, text sizing, responsive behavior, and small visual adjustments.

Avoid custom CSS unless the style is global, repeated, difficult to express clearly with DaisyUI/Tailwind, or already part of the app's established CSS language.

### TypeScript And Linting

Write TypeScript that follows strict ESLint conventions.

Prefer explicit exported types and interfaces for public module boundaries. Avoid `any`; use `unknown` and narrow it when needed. Keep nullable/optional values explicit, and do not silence lint rules unless there is a narrow, documented reason.

Imports should stay clean and intentional:

- Import types with `import type` when the import is type-only.
- Prefer existing path aliases and local project conventions.
- Remove unused imports and dead code as part of the change.

## Migration Guidance

When touching existing client code:

1. Prefer colocating any new files using the target structure.
2. Move existing files only when the move is small and directly related to the change.
3. Update imports in the same commit/change as a move.
4. Avoid broad structure-only refactors unless that is the explicit task.
5. Keep `client/app/page.tsx`, route files, providers, and Next.js framework files simple; they should compose page/domain modules rather than own feature logic.

## Current Structural Assessment

The backend benefits more from its current entity/CQRS structure than from a vertical-slice rewrite.

The client does benefit from a vertical-slice-like approach because UI work usually changes a page, its hooks, its local components, its validation, and its view models together. The best fit here is not a separate `features` folder, but page-area folders under `client/app/pages` plus shared entity folders for reused domain UI and state.
