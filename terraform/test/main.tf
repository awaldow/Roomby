terraform {
  required_version = "> 0.12.0"

  backend "azurerm" {
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = ">= 2.44.0"
    }
  }
}

provider "azurerm" {  
  features {
    key_vault {
      purge_soft_delete_on_destroy = true
    }
  }
}

data "azurerm_client_config" "current" {}

variable "resource_group_name" {
  default = "rg-roomby-test"
  description = "The name of the resource group"
}

variable "resource_group_location" {
  description = "The location of the resource group (West US, Central US, etc.)"
}

variable "app_service_plan_name" {
  default = "asp-roomby-test"
  description = "The name of the app service plan"
}

variable "rooms_app_service_name_prefix" {
  default = "app-roomby-rooms-test"
  description = "The Room app service prefix"
}

variable "users_app_service_name_prefix" {
  default = "app-roomby-users-test"
  description = "The Users app service prefix"
}

variable "rooms_kv_name" {
  default = "kv-roomby-rooms-test"
  description = "The name of the Room API Key Vault"
}

variable "users_kv_name" {
  default = "kv-roomby-users-test"
  description = "The name of the Users API Key Vault"
}

variable "application_insights_name" {
  default = "appi-roomby-test"
  description = "The name of the application insights service for Roomby"
}

variable "storage_account_name" {
  default = "stroombytest"
  description = "The name of the storage account for Roomby"
}

variable "sqlstorage_account_name" {
  default = "stroombysqltest"
  description = "The name of the storage account for the Azure SQL Server"
}

variable "sql_server_name" {
  default = "sqlserver-roomby-test"
  description = "The name of the Azure SQL Server instance for Roomby"
}

variable "rooms_sql_db_name" {
  default = "sql-roomby-rooms-test"
  description = "The name of the Azure SQL DB instance for the Rooms API"
}

variable "users_sql_db_name" {
  default = "sql-roomby-users-test"
  description = "The name of the Azure SQL DB instance for the Users API"
}

variable "sql_server_admin" {
  description = "The name of the SQL server admin account"
}

variable "sql_server_admin_pass" {
  description = "The name of the SQL server admin account password"
}

resource "azurerm_resource_group" "roombytest" {
  name     = var.resource_group_name
  location = var.resource_group_location
}

resource "azurerm_app_service_plan" "roombyplan" {
  name                = var.app_service_plan_name
  location            = azurerm_resource_group.roombytest.location
  resource_group_name = azurerm_resource_group.roombytest.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Basic"
    size = "B1"
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_app_service" "roombyroomstest" {
  name                = var.rooms_app_service_name_prefix
  location            = azurerm_resource_group.roombytest.location
  resource_group_name = azurerm_resource_group.roombytest.name
  app_service_plan_id = azurerm_app_service_plan.roombyplan.id

  site_config {
    linux_fx_version = "DOTNETCORE|3.1"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_app_service" "roombyuserstest" {
  name                = var.users_app_service_name_prefix
  location            = azurerm_resource_group.roombytest.location
  resource_group_name = azurerm_resource_group.roombytest.name
  app_service_plan_id = azurerm_app_service_plan.roombyplan.id

  site_config {
    linux_fx_version = "DOTNETCORE|3.1"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_application_insights" "roombyappi" {
  name                = var.application_insights_name
  location            = azurerm_resource_group.roombytest.location
  resource_group_name = azurerm_resource_group.roombytest.name
  application_type    = "web"

  tags = {
    environment = "test"
  }
}

resource "azurerm_storage_account" "roombystorage" {
  name = var.storage_account_name
  resource_group_name      = azurerm_resource_group.roombytest.name
  location                 = azurerm_resource_group.roombytest.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    environment = "test"
  }
}

resource "azurerm_storage_account" "roombysqlstorage" {
  name = var.sqlstorage_account_name
  resource_group_name      = azurerm_resource_group.roombytest.name
  location                 = azurerm_resource_group.roombytest.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags = {
    environment = "test"
  }
}

resource "azurerm_sql_server" "roombysqlserver" {
  name                         = var.sql_server_name
  resource_group_name          = azurerm_resource_group.roombytest.name
  location                     = azurerm_resource_group.roombytest.location
  version                      = "12.0"
  administrator_login          = var.sql_server_admin
  administrator_login_password = var.sql_server_admin_pass

  tags = {
    environment = "test"
  }
}

resource "azurerm_mssql_database" "roombyusersdb" {
  name                = var.rooms_sql_db_name
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  server_id         = azurerm_sql_server.roombysqlserver.id
  max_size_gb   = 32
  sku_name = "GP_S_Gen5_2"
  storage_account_type = "LRS"
  min_capacity = 1
  auto_pause_delay_in_minutes = 60

  tags = {
    environment = "test"
  }
}

resource "azurerm_mssql_database_extended_auditing_policy" "roombyusersdbauditing" {
  storage_endpoint                        = azurerm_storage_account.roombysqlstorage.primary_blob_endpoint
  storage_account_access_key              = azurerm_storage_account.roombysqlstorage.primary_access_key
  storage_account_access_key_is_secondary = false
  retention_in_days                       = 6
  database_id                             = azurerm_mssql_database.roombyusersdb.id
}

resource "azurerm_mssql_database" "roombyroomsdb" {
  name                = var.users_sql_db_name
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  server_id         = azurerm_sql_server.roombysqlserver.id
  max_size_gb   = 32
  sku_name = "GP_S_Gen5_2"
  storage_account_type = "LRS"
  min_capacity = 1
  auto_pause_delay_in_minutes = 60

  tags = {
    environment = "test"
  }
}

resource "azurerm_mssql_database_extended_auditing_policy" "roombyroomsdbauditing" {
  storage_endpoint                        = azurerm_storage_account.roombysqlstorage.primary_blob_endpoint
  storage_account_access_key              = azurerm_storage_account.roombysqlstorage.primary_access_key
  storage_account_access_key_is_secondary = false
  retention_in_days                       = 6
  database_id                             = azurerm_mssql_database.roombyroomsdb.id
}

resource "azurerm_key_vault" "roombyroomstest" {
  name                        = var.rooms_kv_name
  location                    = azurerm_resource_group.roombytest.location
  resource_group_name         = azurerm_resource_group.roombytest.name
  enabled_for_disk_encryption = false
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "get",
    ]

    secret_permissions = [
      "get", "set", "delete", "purge"
    ]

    storage_permissions = [
      "get",
    ]
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_key_vault_access_policy" "roomsaccesspolicy" {
  key_vault_id = azurerm_key_vault.roombyroomstest.id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = azurerm_app_service.roombyroomstest.identity.0.principal_id

  key_permissions = [ "get", ]
  secret_permissions = [ "get", ]
}

resource "azurerm_key_vault_secret" "roomsappinsightsconnection" {
  name = "ApplicationInsights--ConnectionString"
  value = azurerm_application_insights.roombyappi.connection_string
  key_vault_id = azurerm_key_vault.roombyroomstest.id
}

resource "azurerm_key_vault_secret" "roomssqldbconnection" {
  name = "ConnectionStrings--RoombyRoomSql"
  value = "Server=tcp:${azurerm_sql_server.roombysqlserver.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.roombyroomsdb.name};Persist Security Info=False;User ID=${azurerm_sql_server.roombysqlserver.administrator_login};Password=${azurerm_sql_server.roombysqlserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.roombyroomstest.id
}

resource "azurerm_key_vault" "roombyuserstest" {
  name                        = var.users_kv_name
  location                    = azurerm_resource_group.roombytest.location
  resource_group_name         = azurerm_resource_group.roombytest.name
  enabled_for_disk_encryption = false
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "get",
    ]

    secret_permissions = [
      "get", "set", "delete", "purge"
    ]

    storage_permissions = [
      "get",
    ]
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_key_vault_access_policy" "usersaccesspolicy" {
  key_vault_id = azurerm_key_vault.roombyuserstest.id

  tenant_id                   = data.azurerm_client_config.current.tenant_id
  object_id = azurerm_app_service.roombyuserstest.identity.0.principal_id

  key_permissions = [ "get", ]
  secret_permissions = [ "get", ]
}

resource "azurerm_key_vault_secret" "usersappinsightsconnection" {
  name = "ApplicationInsights--ConnectionString"
  value = azurerm_application_insights.roombyappi.connection_string
  key_vault_id = azurerm_key_vault.roombyuserstest.id
}

resource "azurerm_key_vault_secret" "userssqldbconnection" {
  name = "ConnectionStrings--RoombyRoomSql"
  value = "Server=tcp:${azurerm_sql_server.roombysqlserver.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.roombyusersdb.name};Persist Security Info=False;User ID=${azurerm_sql_server.roombysqlserver.administrator_login};Password=${azurerm_sql_server.roombysqlserver.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.roombyuserstest.id
}

output "instrumentation_key" {
  value = azurerm_application_insights.roombyappi.instrumentation_key
}

output "application_insights_app_id" {
  value = azurerm_application_insights.roombyappi.app_id
}

output "users_app_service_name" {
  value       = azurerm_app_service.roombyuserstest.name
  description = "The App Service name for the test Users API"
}

output "rooms_app_service_name" {
  value       = azurerm_app_service.roombyroomstest.name
  description = "The App Service name for the test Rooms API"
}

output "users_app_service_hostname" {
  value       = azurerm_app_service.roombyuserstest.default_site_hostname
  description = "The hostname of the test Users API"
}

output "rooms_app_service_hostname" {
  value       = azurerm_app_service.roombyroomstest.default_site_hostname
  description = "The hostname of the test Rooms API"
}
