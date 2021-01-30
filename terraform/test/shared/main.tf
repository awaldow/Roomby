terraform {
  required_version = "> 0.13.0"

  backend "azurerm" {
  }

  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "~> 2.44.0"
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
  default = "rg-roomby-users-test"
  description = "The name of the resource group"
}

variable "resource_group_location" {
  description = "The location of the resource group (West US, Central US, etc.)"
}

variable "storage_account_name" {
  default = "stroombytest"
  description = "The name of the storage account for Roomby"
}

variable "tasks_table_name" {
  default = "roombytasks"
}

variable "purchases_table_name" {
  default = "roombypurchases"
}

variable "apim_service_name" {
  default = "roomby-api-test"
}

resource "azurerm_resource_group" "roombytest" {
  name     = var.resource_group_name
  location = var.resource_group_location
}

resource "azurerm_api_management" "roombyapim" {
  name                = var.apim_service_name
  location            = azurerm_resource_group.roombytest.location
  resource_group_name = azurerm_resource_group.roombytest.name
  publisher_name      = "Roomby"
  publisher_email     = "a.wal.bear@gmail.com"

  sku_name = "Consumption_1"

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
