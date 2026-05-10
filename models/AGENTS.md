# models/AGENTS.md

Models are Pydantic classes only. Domain entities (ORM), request bodies, and
response shapes are all separate classes — never share one class across layers.

## Naming
- ORM entity: `{Entity}` in `models/{entity}.py`
- Request body: `Create{Entity}Request`, `Update{Entity}Request`
- Response: `{Entity}Response`

## Structure
```python
# ORM entity (SQLAlchemy)
class {Entity}(Base):
    __tablename__ = "{entities}"
    id: Mapped[UUID] = mapped_column(primary_key=True, default=uuid4)
    name: Mapped[str] = mapped_column(String(200))
    created_at: Mapped[datetime] = mapped_column(default=datetime.utcnow)

# Response schema (Pydantic)
class {Entity}Response(BaseModel):
    id: UUID
    name: str
    created_at: datetime

    model_config = ConfigDict(from_attributes=True)
```

## Rules
- Every ORM entity has `id` (UUID) and `created_at` (datetime UTC)
- Response models always include `model_config = ConfigDict(from_attributes=True)`
- Never return ORM entities directly from a router — map to a response model first
- Request models live in `schemas/` or alongside the router — not in `models/`

## Adding a new entity
1. Create `models/{entity}.py` with the ORM class
2. Import and include in `models/__init__.py` so Alembic detects it
3. Generate migration: `alembic revision --autogenerate -m "add_{entity}"`
