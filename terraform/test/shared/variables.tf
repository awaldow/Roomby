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

variable "users_api_name" {
  default = "users"
}

variable "rooms_api_name" {
  default = "rooms"
}

variable "roomby_product_id" {
    default = "roomby"
}

variable "rooms_app_service_name" {
  default = "app-roomby-rooms-test"
  description = "The Room app service prefix"
}

variable "users_app_service_name" {
  default = "app-roomby-users-test"
  description = "The Users app service prefix"
}

variable "users_app_service_resource_group" {
  default = "rg-roomby-users-test"
  description = "The name of the resource group"
}

variable "rooms_app_service_resource_group" {
  default = "rg-roomby-rooms-test"
  description = "The name of the resource group"
}