# MyUserApi – API z JWT, logowaniem, rejestracją i Dockerem

API napisane w .NET 9.0 – do zarządzania użytkownikami z JWT, rolami, logowaniem i dockerem. Działa z bazą MSSQL w kontenerze. Do tego Serilog do logów i testy jednostkowe w xUnit.

---

## Co tu jest zrobione?

- Rejestracja i logowanie użytkownika
- JWT (token + refresh token)
- Role: `User`, `Admin` – i zabezpieczenie endpointów
- CRUD na użytkownikach
- Paginacja
- Hasła szyfrowane BCryptem
- Serilog – logi lecą do pliku + konsoli
- Swagger z autoryzacją
- Docker + docker-compose
- Testy jednostkowe xUnit

---

## JWT – logowanie i dostęp

- Rejestracja: `POST /api/Auth/register`
- Logowanie: `POST /api/Auth/login`
- Token JWT wklejasz w Swaggerze w `Authorize` jako:

Bearer eyJhbGciOiJIUzI1NiIsInR5...

- `GET /api/Users` – dostępny po zalogowaniu
- `DELETE /api/Users/{id}` – tylko `Admin`

---

## Przykład rejestracji i logowania

```json
// Rejestracja
POST /api/Auth/register
{
  "email": "admin@example.com",
  "firstName": "Admin",
  "lastName": "User",
  "role": "Admin",
  "password": "StrongPassword123!"
}
```

```json
// Logowanie
POST /api/Auth/login
{
  "email": "admin@example.com",
  "password": "StrongPassword123!"
}
```

---

## Docker

W repo jest `docker-compose.yml` – odpalasz nim MSSQL + API.

### Jak odpalić?

```bash
docker-compose down
docker-compose up --build
```

Swagger dostępny będzie pod:
👉 `http://localhost:8080/swagger`

---

## Logi – Serilog

Logi lecą do:

- `logs/log.txt`
- konsoli (stdout)

Loguję wszystko: requesty, błędy, EF Core, migracje itp.

---

## Testy

W katalogu `MyUserApi.Tests` są testy do UserService – robione w xUnit na bazie in-memory.

---

### Jak coś nie działa – sprawdź w logach. Wszystko tam leci
