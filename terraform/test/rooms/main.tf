terraform {
  required_version = "> 0.13.0"

  backend "azurerm" {
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "~> 2.45.1"
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

data "azurerm_resource_group" "roombytest" {
  name     = var.resource_group_name
}

data "azurerm_sql_server" "roombysqlserver" {
  name     = var.sql_server_name
  resource_group_name = var.resource_group_name
}

data "azurerm_app_service_plan" "roombyplan" {
  name     = var.app_service_plan_name
  resource_group_name = var.resource_group_name
}

data "azurerm_application_insights" "roombyappi" {
  name     = var.application_insights_name
  resource_group_name = var.resource_group_name
}

data "azurerm_storage_account" "roombysqlstorage" {
  name     = var.sqlstorage_account_name
  resource_group_name = var.resource_group_name
}

data "azurerm_api_management" "roombyapim" {
  name     = var.apim_service_name
  resource_group_name = var.resource_group_name
}

data "azurerm_api_management_product" "roombyproduct" {
  product_id     = var.roomby_product_id
  api_management_name = var.apim_service_name
  resource_group_name = var.resource_group_name
}

data "azurerm_api_management_api_version_set" "roomsversionset" {
  name     = var.roomby_version_set_name
  api_management_name = var.apim_service_name
  resource_group_name = var.resource_group_name
}

resource "azurerm_mssql_database" "roombyroomsdb" {
  name                = var.rooms_sql_db_name
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  server_id         = data.azurerm_sql_server.roombysqlserver.id
  max_size_gb   = 32
  sku_name = "GP_S_Gen5_2"
  storage_account_type = "GRS"
  min_capacity = 1
  auto_pause_delay_in_minutes = 60

  tags = {
    environment = "test"
  }
}

resource "azurerm_mssql_database_extended_auditing_policy" "roombyroomsdbauditing" {
  storage_endpoint                        = data.azurerm_storage_account.roombysqlstorage.primary_blob_endpoint
  storage_account_access_key              = data.azurerm_storage_account.roombysqlstorage.primary_access_key
  storage_account_access_key_is_secondary = false
  retention_in_days                       = 6
  database_id                             = azurerm_mssql_database.roombyroomsdb.id
}

resource "azurerm_app_service" "roombyroomstest" {
  name                = var.rooms_app_service_name_prefix
  location            = data.azurerm_resource_group.roombytest.location
  resource_group_name = data.azurerm_resource_group.roombytest.name
  app_service_plan_id = data.azurerm_app_service_plan.roombyplan.id
  https_only = true

  site_config {
    dotnet_framework_version = "v5.0"
    windows_fx_version = "DOTNET|5.0"
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = data.azurerm_application_insights.roombyappi.instrumentation_key
    "ASPNETCORE_ENVIRONMENT"             = "Staging"
    "ASPNETCORE_HTTPS_PORT"              = 443
    "WEBSITE_HTTPLOGGING_RETENTION_DAYS" = 1
    "WEBSITE_RUN_FROM_PACKAGE"           = 1
  }

  connection_string {
    name = "RoombyRoomSql"
    type = "SQLAzure"
    value = "Server=tcp:${data.azurerm_sql_server.roombysqlserver.fqdn},1433;Initial Catalog=${azurerm_mssql_database.roombyroomsdb.name};Persist Security Info=False;User ID=${data.azurerm_sql_server.roombysqlserver.administrator_login};Password=${var.sql_server_admin_pass};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_key_vault" "roombyroomstest" {
  name                        = var.rooms_kv_name
  location                    = data.azurerm_resource_group.roombytest.location
  resource_group_name         = data.azurerm_resource_group.roombytest.name
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
  value = data.azurerm_application_insights.roombyappi.connection_string
  key_vault_id = azurerm_key_vault.roombyroomstest.id
}

resource "azurerm_key_vault_secret" "roomssqldbconnection" {
  name = "ConnectionStrings--RoombyRoomSql"
  value = "Server=tcp:${data.azurerm_sql_server.roombysqlserver.fqdn},1433;Initial Catalog=${azurerm_mssql_database.roombyroomsdb.name};Persist Security Info=False;User ID=${data.azurerm_sql_server.roombysqlserver.administrator_login};Password=${var.sql_server_admin_pass};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.roombyroomstest.id
}

output "rooms_app_service_name" {
  value       = azurerm_app_service.roombyroomstest.name
  description = "The App Service name for the test Rooms API"
}

output "rooms_app_service_hostname" {
  value       = azurerm_app_service.roombyroomstest.default_site_hostname
  description = "The hostname of the test Rooms API"
}
