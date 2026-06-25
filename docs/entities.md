# Схема сущностей

## Иерархия

```
City (Город)
├── District[] (Район)
│   ├── Building[] (Здание)
│   │   └── Slot[] (квартира / рабочее место / столик)
│   └── stats (prestige / safety / transport / ecology / ...)
└── CityEconomy (рыночные условия, кризисы, инфляция)

TimeSystem (Singleton)
├── currentDateTime
├── speed (Paused / Normal / Fast / VeryFast)
└── calendar

Family (Семья)
├── name
├── dynastyHistory[]
└── FamilyMember[] (до 10 персонажей)
    ├── Identity (имя, возраст, внешность)
    ├── LifeStats (HP, MP) ← смерть при 0
    ├── Needs (Nutrition, Energy, Mood, Stress) ← дренируют HP/MP при нехватке
    ├── Characteristics{} (18 врождённых черт, 0–100)
    ├── Skills{} (навык → уровень)
    ├── Habits[] (привычки)
    ├── Relationships{} (персонаж → RelationshipData)
    ├── Reputation (профессиональная, деловая, социальная)
    ├── LegalStatus (документы, дела, штрафы, судимости)
    ├── FinancialObligations[] → Debt | Fine
    ├── PersonalFinances (деньги, счета, акции, имущество, бизнесы)
    ├── currentTask → Task
    └── location → Building

NPC
├── Identity (упрощённая)
├── role (коллега / сосед / работодатель / чиновник...)
├── schedule[]
└── simplified Health & Needs

Corporation[]
├── finances (revenue, expenses, profit)
├── Stock (price, priceHistory[])
├── Buildings[] → Building
└── JobPositions[] → JobPosition

PlayerBusiness
├── type
├── building → Building
├── owner → FamilyMember
├── employees[]
├── licenses[] → License
├── finances
└── inspectionRisk

WeeklyPlan
├── family → Family
├── weekStart
└── priorities[] → Priority (тип + персонаж + вес)

Task
├── type
├── targetBuilding → Building
├── assignedCharacter → FamilyMember
├── startTime / duration / progress
├── requirements (skills, money, documents, relationships)
└── outcomes (money, skillXP, relationshipChange, healthChange, stressChange, eventTrigger)

Event
├── type (финансовое / правовое / социальное / городское...)
├── scope (город / район / семья / персонаж)
├── trigger (условие запуска)
├── choices[] (варианты решения с последствиями)
└── effects[] (что меняется)

Document
├── type / owner / issuer
└── status (действителен / просрочен / аннулирован)

License
├── type / business
└── status (активна / приостановлена / отозвана)

LegalCase
├── type / parties[] / subject
├── status
└── possibleOutcomes[]

Fine
├── reason / amount / dueDate
└── status

Debt
├── creditor / principal / interestRate
├── monthlyPayment / remainingMonths
├── collateral
└── penaltyOnDefault

Scenario (шаблон начала игры)
├── name / description
├── startDistrict / startHousing
├── familyTemplate[]
├── cash / debts[]
├── assets[] / jobs[]
├── reputation / legalProblems[]
└── startingOpportunities[] / socialNetwork[]
```

---

## Ключевые связи

```
FamilyMember ──работает в──────► JobPosition ──принадлежит──► Corporation
FamilyMember ──живёт в─────────► Building (квартира / дом)
FamilyMember ──владеет─────────► Building | PlayerBusiness | акции Corporation
FamilyMember ──выполняет───────► Task ──происходит в──► Building
FamilyMember ──связан с────────► FamilyMember | NPC (через Relationships)
FamilyMember ──имеет───────────► LegalStatus (Document[], LegalCase[], Fine[])
FamilyMember ──имеет───────────► Habit[] ──модифицирует──► Task outcomes
Corporation ───владеет─────────► Building (офисы, заводы)
PlayerBusiness ─находится в───► Building
PlayerBusiness ─требует────────► License[]
Event ──────────────────────────► влияет на Corporation.finances | District.stats | FamilyMember.health
WeeklyPlan ─────────────────────► определяет поведение FamilyMember при отсутствии прямых команд
TimeSystem ─────────────────────► тикает Task.progress | Corporation.finances | NPC.schedule | Debt.payments
```

---

## Что влияет на что (упрощённо)

| Источник | Влияет на |
|---|---|
| Characteristics (трудолюбие) | Скорость и качество рабочих задач |
| Characteristics (бюрократизм) | Скорость работы с документами |
| Characteristics (отношение к закону) | Склонность к нарушениям, реакция на проверки |
| Characteristics (любовь к общению) | Скорость роста Relationships.trust |
| Characteristics (отношение к комфорту) | Стресс от плохого жилья / транспорта |
| Skills (программирование) | Доступ к IT-вакансиям + качество работы |
| Habits | Вероятность выбора действий в свободное время |
| Health.stress | Events, ухудшение HP при превышении порога |
| District.transportQuality | Время и стоимость маршрутов |
| District.safety | Риск событий в поездках |
| Building.condition | Функциональность и стоимость |
| Corporation.finances | Stock.price, количество вакансий |
| CityEconomy | housingPrice, rentLevel, зарплаты, спрос на бизнес |
| WeeklyPlan | Поведение персонажа по умолчанию |
| LegalCase | Репутация, финансы, свобода персонажа |
| Debt.penaltyOnDefault | Изъятие залога, суд, кредитная история |
| TimeSystem | Всё что требует времени |
