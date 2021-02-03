resource "azurerm_api_management_api" "roombyusersapi_rev1" {
	name	=	"roombyusersapi_rev1"
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"Roomby Users API"
	revision	=	"1"
	version	=	"1"
	version_set_id	=	data.azurerm_api_management_product.usersversionset.id
	path	=	"users"
	protocols	=	["https"]
	service_url	=	"${azurerm_app_service.roombyuserstest.default_site_hostname}/api/"
}

resource "azurerm_api_management_product_api" "roombyusersapi_rev1productapi" {
	api_name	=	azurerm_api_management_api.roombyusersapi_rev1.name
	product_id	=	data.azurerm_api_management_product.roombyproduct.product_id
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
}
