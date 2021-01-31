resource "azurerm_api_management" "roombyapim" {
  name                = var.apim_service_name
  location            = azurerm_resource_group.roombytest.location
  resource_group_name = azurerm_resource_group.roombytest.name
  publisher_name      = "Roomby"
  publisher_email     = "a.wal.bear@gmail.com"

  sku_name = "Consumption_1"

  identity {
    type = "SystemAssigned"
  }

  tags = {
    environment = "test"
  }
}