```sql
CREATE TABLE JournalProperties (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalEntryId INTEGER NOT NULL REFERENCES JournalEntries (Id),
  PropertyName TEXT NOT NULL,
  SubPropertyName TEXT,
  Value VARIANT
)
CREATE INDEX JournalProperty_JournalEntryId ON JournalProperties (JournalEntryId)
CREATE INDEX JournalProperty_PropertyName ON JournalProperties (PropertyName)
```

This table contains data from the journal entries, for entries we want to process, display or join on.

* `PropertyName`: The name of the key in the journal entry JSON
* `SubPropertyName`: Used for properties that are objects (e.g. `Materials`) or arrays of objects (e.g. `Killers`)
* `Value`: The value of the property (or sub-property when the property is an object or array of objects)
