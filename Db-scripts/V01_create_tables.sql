CREATE TABLE "account" (
	"id" INTEGER PRIMARY KEY AUTOINCREMENT,
	"name" TEXT NOT NULL,
	"timestamp" DATETIME NOT NULL
);


CREATE TABLE "currency" (
	"id" INTEGER PRIMARY KEY AUTOINCREMENT,
	"name" TEXT NOT NULL,
	"timestamp" DATETIME NOT NULL
);


CREATE TABLE "category" (
	"id" INTEGER PRIMARY KEY AUTOINCREMENT,
	"name" TEXT NOT NULL,
	"timestamp" DATETIME NOT NULL
);

CREATE TABLE "trans" (
    "id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "timestamp" DATETIME NOT NULL,
    "account" INTEGER NOT NULL, 
    "category" INTEGER NOT NULL,
    "currency" INTEGER NOT NULL,
    "amount" INTEGER NOT NULL,
    "balance" INTEGER,
    "description" TEXT,
    "note" TEXT,
    FOREIGN KEY (account) REFERENCES account(id),
    FOREIGN KEY (category) REFERENCES category(id),
    FOREIGN KEY (currency) REFERENCES currency(id)
);