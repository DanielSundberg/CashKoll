# CashKoll
Personal finance tracker created with simplicity and privacy in mind (the data never leaves your computer).

Port to .Net 7 of the Excel sheet I've been using to track my personal finances.

## Features
* Show cash spent on different categories of expenses
* Local database
* CSV import of data, Nordea CSV format supported by default
* Using Blazor for UI
* Using local SQLite database for data storage 

## Screeshot

![Screenshot of main screen](main-screenshot.png)

## How to run

* Create sqlite database (using for example <a href="https://www.heidisql.com/">HeidiSQL</a>).
* Run scripts in /Db-scripts:
    * V01_create_tables.sql
    * V02_create_category_map.sql
    * V03_trans_deleted_column.sql
* Optionally run S** scripts to create sample data or add your own
* Add database file, expense and savings account id to your appsettings.json

````
  "ExpenseAccountId": 1,
  "SavingsAccountId": 3,
  "ConnectionString": "Data Source=C:\\temp\\CashKoll-Db\\cashkoll.sqlite3"
````

* Build and run the app
````
> dotnet build
> dotnet run
````

Now your browser will start.