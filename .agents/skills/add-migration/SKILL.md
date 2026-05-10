---
name: add-migration
description: Add a new SQLAlchemy entity and create an Alembic migration
---

# Skill: Add Migration

## Steps

1. **Create the entity** in `models/{entity}.py`
   - Inherit from `Base`
   - Include `id` (UUID, primary key) and `created_at` (datetime UTC)
   - Use `Mapped[T]` type annotations throughout

2. **Register the model** — import the new class in `models/__init__.py`
   so Alembic's autogenerate detects it

3. **Generate the migration**
   ```bash
   alembic revision --autogenerate -m "add_{entity}"
   ```
   Review the generated file in `alembic/versions/` — confirm columns and
   types look correct before finishing.

4. **Verify**: `pytest` exits 0

## Do not
- Manually edit the generated migration file unless autogenerate got it wrong
- Run `alembic upgrade head` — migrations apply on startup
- Add business logic or computed values to the migration
