variable "resource_group_name" {
  default = "rg-roomby-test"
  description = "The name of the resource group"
}

variable "resource_group_location" {
  description = "The location of the resource group (West US, Central US, etc.)"
}

variable "users_app_service_name_prefix" {
  default = "app-roomby-users-test"
  description = "The Users app service prefix"
}

variable "users_kv_name" {
  default = "kv-roomby-users-test"
  description = "The name of the Users API Key Vault"
}

variable "sqlstorage_account_name" {
  default = "stroombysqltest"
  description = "The name of the storage account for the Azure SQL Server"
}

variable "users_sql_db_name" {
  default = "sql-roomby-users-test"
  description = "The name of the Azure SQL DB instance for the Users API"
}

variable "app_service_plan_name" {
  default = "asp-roomby-test"
  description = "The name of the app service plan"
}

variable "application_insights_name" {
  default = "appi-roomby-test"
  description = "The name of the application insights service for Roomby"
}