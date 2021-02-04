resource "azurerm_api_management_api_operation" "GetHouseholdAsync" {
	operation_id	=	"GetHouseholdAsync"
	api_name	=	azurerm_api_management_api.roombyusersapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"GetHouseholdAsync(Guid householdId)"
	method	=	"Get"
	url_template	=	"/api/v1/Households/{householdId}"
	description	=	"Returns the Household object for householdId"
	template_parameter {
		name	=	"householdId"
		required	=	true
		type	=	"uuid"
		description	=	"Household ID for the Household to get"
	}
	response {
		status_code	=	401
		description	=	"Unauthorized"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	403
		description	=	"Forbidden"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	500
		description	=	"Server Error"
	}
	response {
		status_code	=	200
		description	=	"Success"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	404
		description	=	"Not Found"
		representation {
			content_type	=	"application/json"
		}
	}
}
resource "azurerm_api_management_api_operation" "CreateHouseholdAsync" {
	operation_id	=	"CreateHouseholdAsync"
	api_name	=	azurerm_api_management_api.roombyusersapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"CreateHouseholdAsync(Household householdToCreate)"
	method	=	"Post"
	url_template	=	"/api/v1/Households"
	description	=	"Creates the provided householdToCreate"
	response {
		status_code	=	401
		description	=	"Unauthorized"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	403
		description	=	"Forbidden"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	500
		description	=	"Server Error"
	}
	response {
		status_code	=	400
		description	=	"Bad Request"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	201
		description	=	"Success"
		representation {
			content_type	=	"application/json"
		}
	}
}
resource "azurerm_api_management_api_operation" "GetUserAsync" {
	operation_id	=	"GetUserAsync"
	api_name	=	azurerm_api_management_api.roombyusersapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"GetUserAsync(Guid userId)"
	method	=	"Get"
	url_template	=	"/api/v1/Users/{userId}"
	description	=	"Returns the User object for userId"
	template_parameter {
		name	=	"userId"
		required	=	true
		type	=	"uuid"
		description	=	"User ID for the User to get"
	}
	response {
		status_code	=	401
		description	=	"Unauthorized"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	403
		description	=	"Forbidden"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	500
		description	=	"Server Error"
	}
	response {
		status_code	=	200
		description	=	"Success"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	404
		description	=	"Not Found"
		representation {
			content_type	=	"application/json"
		}
	}
}

