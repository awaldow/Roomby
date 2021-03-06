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

resource "azurerm_resource_group" "roombytest" {
  name     = var.resource_group_name
  location = var.resource_group_location
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

resource "azurerm_app_service_plan" "roombyplan" {
  name                = var.app_service_plan_name
  location            = azurerm_resource_group.roombytest.location
  resource_group_name = azurerm_resource_group.roombytest.name

  sku {
    tier = "Basic"
    size = "B1"
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_storage_account" "roombydocs" {
  name = var.docs_storage_account_name
  resource_group_name      = azurerm_resource_group.roombytest.name
  location                 = azurerm_resource_group.roombytest.location
  account_tier             = "Standard"
  account_replication_type = "RAGRS"
  enable_https_traffic_only = true

  static_website {
    index_document = "index.html"
  }

  tags = {
    environment = "test"
  }
}

resource "azurerm_storage_account" "roombysqlstorage" {
  name = var.sqlstorage_account_name
  resource_group_name      = azurerm_resource_group.roombytest.name
  location                 = azurerm_resource_group.roombytest.location
  account_tier             = "Standard"
  account_replication_type = "RAGRS"

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

resource "azurerm_storage_table" "roombytaskstable" {
  name = var.tasks_table_name
  storage_account_name = azurerm_storage_account.roombystorage.name
}

resource "azurerm_storage_table" "roombypurchasestable" {
  name = var.purchases_table_name
  storage_account_name = azurerm_storage_account.roombystorage.name
}

resource "azurerm_storage_container" "roombyicons" {
  name                  = "icons"
  storage_account_name  = azurerm_storage_account.roombystorage.name
  container_access_type = "private"
}

output "resource_group" {
  value = var.resource_group_name
}
