variable "resource_group_name" {
  default = "rg-roomby-test"
  description = "The name of the resource group"
}

variable "resource_group_location" {
  description = "The location of the resource group (West US, Central US, etc.)"
}

variable "rooms_app_service_name_prefix" {
  default = "app-roomby-rooms-test"
  description = "The Room app service prefix"
}

variable "rooms_kv_name" {
  default = "kv-roomby-rooms-test"
  description = "The name of the Room API Key Vault"
}

variable "sqlstorage_account_name" {
  default = "stroombysqltest"
  description = "The name of the storage account for the Azure SQL Server"
}

variable "rooms_sql_db_name" {
  default = "sql-roomby-rooms-test"
  description = "The name of the Azure SQL DB instance for the Rooms API"
}

variable "app_service_plan_name" {
  default = "asp-roomby-test"
  description = "The name of the app service plan"
}

variable "application_insights_name" {
  default = "appi-roomby-test"
  description = "The name of the application insights service for Roomby"
}

variable "apim_service_name" {
  default = "roomby-api-test"
}

variable "roomby_product_id" {
    default = "roomby"
}

variable "roomby_version_set_name" {
    default = "rooms"
}

variable "sql_server_name" {
  default = "sqlserver-roomby-test"
  description = "The name of the Azure SQL Server instance for Roomby"
}

variable "sql_server_admin_pass" {
  description = "The name of the SQL server admin account password"
}
