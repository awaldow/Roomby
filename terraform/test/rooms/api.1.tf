resource "azurerm_api_management_api" "roombyroomsapi_rev1" {
	name	=	"roombyroomsapi_rev1"
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"Roomby Rooms API"
	revision	=	"1"
	version	=	"v1"
	version_set_id	=	data.azurerm_api_management_api_version_set.roomsversionset.id
	path	=	"rooms"
	protocols	=	["https"]
	service_url	=	"https://${azurerm_app_service.roombyroomstest.default_site_hostname}/api/v1/"
}

resource "azurerm_api_management_product_api" "roombyroomsapi_rev1productapi" {
	api_name	=	azurerm_api_management_api.roombyroomsapi_rev1.name
	product_id	=	data.azurerm_api_management_product.roombyproduct.product_id
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
}

