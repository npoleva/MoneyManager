# MoneyManager — Domain-Driven Design (DDD)

**MoneyManager** — приложение для управления личными финансами, построенное с использованием принципов DDD.

---

## Цель

Приложение отделяет бизнес-логику от инфраструктуры и презентации.

Основные цели:
- Чистая модель предметной области
- Ясный Ubiquitous Language
- Возможность масштабирования и тестирования

---

## Domain Layer

### Агрегаты

**Wallet (Кошелек)**

- **Роль:** корневой агрегат, управляющий балансом и транзакциями
- **Зачем:** обеспечивает консистентность бизнес-правил (например, нельзя потратить больше, чем есть)
- **Содержит:**
    - Сущности `Transaction`
    - Value Objects `Money` и `TransactionDescription`

```text
Wallet
├─ Id : Guid
├─ Name : string
├─ InitialBalance : Money
├─ Transactions : List<Transaction>
├─ CurrentBalance : Money (вычисляемое)
└─ Методы:
    ├─ AddTransaction(Transaction)
    ├─ GetTransactionsByMonthAndType(month, year, type)
    ├─ GetTopExpenses(month, year)
    └─ GetTransactionsGroupedByType(month, year)
