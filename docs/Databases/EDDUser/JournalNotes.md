# EDDUser.JournalNotes table

```sql
CREATE TABLE JournalNotes (
  id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntry (Id),
  Time DATETIME NOT NULL, 
  Note TEXT
)
CREATE INDEX SystemNote_JournalEntryId ON JournalNotes (JournalEntryId)
```

Linked to journal entries by the `JournalEntryId` column.
