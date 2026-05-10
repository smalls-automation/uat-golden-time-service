# AGENTS.md — smalls-automation/uat-golden-time-service

## Stack
- Language: Python 3.12+
- Framework: FastAPI
- ORM: SQLAlchemy + Alembic migrations

## Commands
- Run: `uvicorn main:app --reload`
- Test: `pytest`
- Lint: `ruff check .`
- New migration: `alembic revision --autogenerate -m "{description}"`

## Project structure
```
routers/     ← HTTP entry points (see routers/AGENTS.md)
services/    ← business logic + interfaces (see services/AGENTS.md)
models/      ← SQLAlchemy ORM entities (see models/AGENTS.md)
schemas/     ← Pydantic request/response models
dependencies.py  ← FastAPI Depends providers
main.py      ← app creation and router registration
```

## Rules
1. Read the AGENTS.md in the directory you are working in before writing code
2. Follow existing patterns — find a similar file and match its structure exactly
3. Never modify `tests/` unless the task explicitly says to write tests
4. `pytest` must exit 0 before you finish
5. Register new routers in `main.py` following the existing pattern

## Boundaries
- Do not modify `alembic/`, `.github/`, `Dockerfile`, or `docker-compose.yml`
- Do not add dependencies to `requirements.txt` without explicit instruction
