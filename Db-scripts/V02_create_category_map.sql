CREATE TABLE "category_map" (
	"id" INTEGER PRIMARY KEY AUTOINCREMENT,
    "category" INTEGER NOT NULL,
	"name" TEXT,
    "type" INTEGER,
    "expression" TEXT,
	"timestamp" DATETIME NOT NULL
    FOREIGN KEY (category) REFERENCES category(id)
);
