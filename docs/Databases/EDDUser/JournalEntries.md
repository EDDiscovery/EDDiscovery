# EDDUser.JournalEntries table

```sql
CREATE TABLE JournalEntries (
  Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  JournalId INTEGER NOT NULL REFERENCES Journals (Id),
  EventTypeId INTEGER NOT NULL,
  EventType TEXT,
  EventTime DATETIME NOT NULL,
  EventData TEXT, -- JSON String of complete line
  EdsmId INTEGER, -- 0 if not set yet.
  Synced INTEGER,
)

CREATE INDEX JournalEntry_JournalId ON JournalEntries (JournalId)
CREATE INDEX JournalEntry_EventTypeId ON JournalEntries (EventTypeId)
CREATE INDEX JournalEntry_EventType ON JournalEntries (EventType)
CREATE INDEX JournalEntry_EventTime ON JournalEntries (EventTime)
```

This table contains entries from the journal or converted entries from the pre-2.2 netlogs.

All events we want to understand, at journal entry, their parameters are decoded and stored into `JournalProperties` table. Only certain journal entries are to be initially decoded and expanded and stored.

Additionally, FSDjumps get expanded into JournalFSDJumps table AND Journal Properties.  The reason for the second table is dB lookup speed.
