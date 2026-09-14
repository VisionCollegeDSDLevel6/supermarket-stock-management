# Thanh Testing Work Allocation

**Role:** Frontend Developer and QA Tester

This document tracks Thanh's allocated testing work for Week 5 — Testing and Finalisation.

## Summary

| Area | Planned Tests | Owner |
|------|---------------|-------|
| Backend API — Stocks, Staff, Contact | T01–T18 | Thanh |
| React Customer Frontend | F01–F18 | Thanh |
| Security — customer-facing | S04–S06 | Thanh |
| User Acceptance Testing | UAT03, UAT06, UAT07, UAT08 | Thanh |

---

## Backend Tests (T01–T18)

### Stocks API
| ID | Component | Expected Test Outcome |
|----|-----------|----------------------|
| T01 | GET /api/stocks | Returns HTTP 200 with product data |
| T02 | GET /api/stocks/{id} | Existing record returns the correct record |
| T03 | GET /api/stocks/{id} | Missing record returns HTTP 404 |
| T04 | POST /api/stocks | Valid stock returns HTTP 201 |
| T05 | POST /api/stocks | Negative quantity returns HTTP 400 |
| T06 | POST /api/stocks | Negative low-stock threshold returns HTTP 400 |
| T07 | POST /api/stocks | Duplicate stock for a product returns HTTP 409 |
| T08 | PUT /api/stocks/{id} | Valid update returns HTTP 204 |
| T09 | PUT /api/stocks/{id} | Route ID differs from StockId returns HTTP 400 |
| T10 | DELETE /api/stocks/{id} | Deleting existing stock returns HTTP 204 |

### Staff API
| ID | Component | Expected Test Outcome |
|----|-----------|----------------------|
| T11 | GET /api/staffs | An Administrator can view all staff accounts |
| T12 | GET /api/staffs | A Manager can view Staff accounts only |
| T13 | GET /api/staffs/{id} | A Manager is forbidden from viewing an Administrator/Manager record |
| T14 | GET /api/staffs | A Staff user cannot access the staff administration API |
| T15 | GET /api/staffs/{id} | A missing staff ID returns HTTP 404 |

### Contact API
| ID | Component | Expected Test Outcome |
|----|-----------|----------------------|
| T16 | POST /api/contact | A valid contact request returns HTTP 200 and is saved |
| T17 | POST /api/contact | Missing required fields return HTTP 400 |
| T18 | POST /api/contact | Saved values are trimmed and IsRead defaults to false |

---

## Frontend Tests (F01–F18)

| ID | Test | Expected Result |
|----|------|-----------------|
| F01 | Home page loads and main navigation links work | Nav links render and navigate |
| F02 | Product list loads data from the backend API | Products displayed from API |
| F03 | A loading state is shown while data is being retrieved | Spinner shown while loading |
| F04 | An appropriate message is displayed when the API request fails | Error message shown |
| F05 | Product search returns matching products | Filtered results shown |
| F06 | Category filtering displays products from the selected category | Filtered results shown |
| F07 | Products can be sorted alphabetically | A–Z ordering applied |
| F08 | Products can be sorted by price from high to low | High→low ordering applied |
| F09 | Product details show correct name, price, category, image and stock status | All fields shown |
| F10 | An out-of-stock product cannot be added to the cart | Add button disabled/absent |
| F11 | A product can be added to the cart | Item added to cart |
| F12 | Adding the same product increases its quantity | Quantity increments |
| F13 | Cart quantity can be increased and decreased | Quantity adjusts |
| F14 | Reducing quantity to zero removes the item | Item removed |
| F15 | The cart total is calculated correctly | Total matches sum |
| F16 | Cart data remains available after refreshing the browser | Persisted via localStorage |
| F17 | The contact form handles successful and unsuccessful submissions | Success/error messages |
| F18 | Login displays the correct result for valid and invalid credentials | Success/failure shown |

---

## Security Tests (S04–S06)

| ID | Security Test | Expected Outcome |
|----|---------------|------------------|
| S04 | Run OWASP ZAP against the React customer frontend and public API routes | No unresolved high-risk alerts |
| S05 | Submit SQL injection payloads through the customer contact form | Payload does not execute, no database error exposed |
| S06 | Check public and protected routes without valid authorisation | Public data readable; protected operations return 401/403 |

---

## User Acceptance Testing (UAT)

| ID | Acceptance Scenario | Acceptance Criteria | Owner |
|----|---------------------|---------------------|-------|
| UAT03 | Staff user accesses the administration site | Permitted pages available, restricted pages display Access Denied | Thanh |
| UAT06 | Customer browses, searches, filters and sorts products | Relevant products displayed with correct details, price, image and availability | Thanh |
| UAT07 | Customer adds available products to the cart | Cart quantities, removal behaviour and total price correct after refresh | Thanh |
| UAT08 | Customer submits the contact form | Valid input saved; invalid input produces clear validation message | Thanh |

---

## Tools and Evidence

- **Jest** + **React Testing Library** — automated frontend component and interaction tests (F01–F18)
- **xUnit** + EF Core test database — automated backend controller/API tests (T01–T18)
- **Browser test checklist** — manual end-to-end testing of navigation, forms, cart and responsive behaviour
- **OWASP ZAP** — automated vulnerability scanning of the customer frontend and public API
- **SQL injection test cases** — confirm user input cannot bypass auth or alter queries
- **Defect log** — record test ID, issue, severity, owner, fix and retest result

Each test must record its test ID, preconditions, input/action, expected result, actual result,
pass/fail status, tester name and evidence. Failed tests enter the defect log and are retested after fix.