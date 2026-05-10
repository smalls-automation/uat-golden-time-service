# routers/AGENTS.md

Routers are thin HTTP entry points. Validate input via Pydantic models, delegate
to one service, return a typed response. No business logic here.

## Naming
- File: `{resource}.py` (plural snake_case: `users.py`, `books.py`)
- Router: `router = APIRouter(prefix="/{resources}", tags=["{Resources}"])`

## Structure
```python
router = APIRouter(prefix="/{resources}", tags=["{Resources}"])

@router.get("/{id}", response_model={Resource}Response)
async def get_{resource}(
    id: UUID,
    service: Annotated[I{Resource}Service, Depends(get_{resource}_service)],
) -> {Resource}Response:
    result = await service.get_async(id)
    match result:
        case Ok(value):
            return value
        case Err(error):
            raise HTTPException(status_code=404, detail=error)
```

## Rules
- Use `Annotated[..., Depends(...)]` for all dependency injection
- Return typed response models — never raw dicts
- Pattern match on `Result` types at the router boundary — never let `Err` propagate
- No database calls inside routers

## Adding a new router
1. Create `routers/{resource}.py`
2. Register in `main.py`: `app.include_router({resource}.router)`
