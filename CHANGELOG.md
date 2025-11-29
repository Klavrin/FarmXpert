# FarmXpert Changelog

Recent updates make FarmXpert more useful and social. We’ve improved how subsidy document requirements are shown by adding clearer subsidy types and subtypes, so the information is easier to understand. The home page now includes reactions, trending posts, and a friend list, while the backend is more stable thanks to new tests and better domain logic.

## Release: [0.3.0] - 2025-11-29

### Added
- Dark theme support. ([PR #39 – Add dark theme](https://github.com/Klavrin/FarmXpert/pull/39))
- Interactive Leaflet map at `/map`. ([PR #40 – Add leaflet map to /map](https://github.com/Klavrin/FarmXpert/pull/40))
- My Documents page UI providing listing and management interface for user documents. ([PR #42 – My documents page](https://github.com/Klavrin/FarmXpert/pull/42))
- Frontend UX/UI refinements. ([PR #23 – Improve frontend](https://github.com/Klavrin/FarmXpert/pull/23))
- Unit tests. ([PR #22 – Add unit tests](https://github.com/Klavrin/FarmXpert/pull/22))
- Deployment: https://farmxpert-fbdre0dsfbbbdrd4.westeurope-01.azurewebsites.net/Account/Login?ReturnUrl=%2F
- New /profile page ([PR #44 - Add /profile page](https://github.com/Klavrin/FarmXpert/pull/44))

### Changed
- Homepage layout. ([PR #24 – Homepage](https://github.com/Klavrin/FarmXpert/pull/24))
- Enforced official Microsoft .NET formatting standards across the codebase. ([PR #27 – Enforce code formatting](https://github.com/Klavrin/FarmXpert/pull/27))
- CI runner switched from Windows to Ubuntu for speed. ([PR #29 – Switch CI runner from windows to ubuntu](https://github.com/Klavrin/FarmXpert/pull/29))
- Workflow to include unit testing and conditional deployment. ([PR #28 – Update CI workflow](https://github.com/Klavrin/FarmXpert/pull/28))
- 
### Fixed
- Remote connection string initialization issue. ([PR #34](https://github.com/Klavrin/FarmXpert/pull/34))
- Fallback logic for unidentified database name. ([PR #35](https://github.com/Klavrin/FarmXpert/pull/35))
- Minor build warnings resolved. ([PR #36](https://github.com/Klavrin/FarmXpert/pull/36))

### Full References (PRs in this release window)
- #42 My documents page: https://github.com/Klavrin/FarmXpert/pull/42
- #40 Leaflet map: https://github.com/Klavrin/FarmXpert/pull/40
- #39 Dark theme: https://github.com/Klavrin/FarmXpert/pull/39
- #23 Improve frontend: https://github.com/Klavrin/FarmXpert/pull/23
- #24 Homepage: https://github.com/Klavrin/FarmXpert/pull/24
- #27 Code formatting enforcement: https://github.com/Klavrin/FarmXpert/pull/27
- #28 CI workflow update: https://github.com/Klavrin/FarmXpert/pull/28
- #29 CI runner switch: https://github.com/Klavrin/FarmXpert/pull/29
- #34 Remote connection string fix: https://github.com/Klavrin/FarmXpert/pull/34
- #35 DB name fallback: https://github.com/Klavrin/FarmXpert/pull/35
- #36 Build warnings fixes: https://github.com/Klavrin/FarmXpert/pull/36

### Pending (Need Details to Add Later)
- Upload Document page (branch: gabi/upload-document-page) – awaiting PR number, scope, and feature description.

## Release: [0.2.1] - 2025-11-01

### Added
- Unit test suite for command and query handlers of Vehicle, Animal, and Field, plus tests for FileStorageService (file stream recording helper).
  - PR: https://github.com/Klavrin/FarmXpert/pull/22 (merged)

## Release: [0.2.0] - 2025-10-31

### Added
- Application Document controller (CRUD, local file storage, download endpoint).
- Personal Document controller (CRUD, local file storage, download endpoint).
- Social Posts controller:
  - CRUD for posts (image/video), comments, and likes.
  - Queries: get all posts, get post by ID, get posts by user.
  - Infrastructure: inject ISocialPostRepository → SocialPostRepository.
  - Domain: add FileExtension and Likes validation, add BusinessId to SocialPost and Comment for multi-tenant scoping.
- More AIPA documents.
- Subsidy subtypes.

### Changed
- Subsidy type documents.
- Subsidy application process.

### Fixed
- MudMenu not being positioned properly by default.

### References (PRs)
- Documents model: https://github.com/Klavrin/FarmXpert/pull/5
- ApplicationDocument model: https://github.com/Klavrin/FarmXpert/pull/6
- Social posts model and endpoints: https://github.com/Klavrin/FarmXpert/pull/7

---

## Release: [0.1.0] - 2025-10-03

### Added
- Twitter-like home page.
- Subsidy selection page.
- `My Documents` page.
- History of all applications.
- Recommended subsidies.
- Animals controller.
- Fields controller (with multi-tenancy for API endpoints).
- Vehicle controller.
- Core domain scaffolding and initial entities/enums.

### Security
- Added authorization.
- Added password reset using Google SMTP.

### References (PRs)
- Initial entities/enums: https://github.com/Klavrin/FarmXpert/pull/2
- Field entity + multi-tenancy: https://github.com/Klavrin/FarmXpert/pull/3
- Small improvements: https://github.com/Klavrin/FarmXpert/pull/4
- add first enum + entity: https://github.com/Klavrin/FarmXpert/pull/1

---

## 2025-10-15 — Social posts (v1)

- Feature: Social media posts with full CRUD and engagement.
  - Commands/ops: Create post (image/video), Edit post, Delete post, Add comment, Delete comment, Add like, Delete like.
  - Queries: Get all posts, Get post by ID, Get posts by user.
  - API: Add SocialPosts controller.
  - Infrastructure: Inject ISocialPostRepository → SocialPostRepository.
  - Domain: Add attributes for FileExtension, Likes (validation), and BusinessId (multi-tenant scoping). BusinessId added to Comment as well.
  - Note: Ensure User has non-null FirstName, LastName, BusinessId to avoid null properties in comments/posts.
  - PR: https://github.com/Klavrin/FarmXpert/pull/7 (merged)

## 2025-10-06 — Documents and application documents

- Feature: ApplicationDocument model with CRUD; files stored locally; download endpoint.
  - API: ApplicationDocument controller.
  - Server: Inject IApplicationDocumentRepository → ApplicationDocumentRepository.
  - Validation: Enforce PDF format; fix .pdf validation bug.
  - PR: https://github.com/Klavrin/FarmXpert/pull/6 (merged)

- Feature: Documents model with CRUD; files stored in local FileStorage directory; download endpoint.
  - PR: https://github.com/Klavrin/FarmXpert/pull/5 (merged)

## 2025-10-02 — Small improvements

- General code and quality improvements.
  - PR: https://github.com/Klavrin/FarmXpert/pull/4 (merged)

## 2025-09-30 — Field entity

- Feature: Fully implement Field entity to all API endpoints.
  - PR: https://github.com/Klavrin/FarmXpert/pull/3 (merged)

## 2025-09-25 — Initial entities

- Feature: Add all enums and core entities to establish the domain model.
  - PR: https://github.com/Klavrin/FarmXpert/pull/2 (merged)

## 2025-09-24 — Project bootstrap

- Initial groundwork: First enum and entity added to start the project structure.
  - PR: https://github.com/Klavrin/FarmXpert/pull/1 (merged)