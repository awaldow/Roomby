resource "azurerm_api_management_api_operation" "GetRoomsForHouseholdAsync" {
	operation_id	=	"GetRoomsForHouseholdAsync"
	api_name	=	azurerm_api_management_api.roombyroomsapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"GetRoomsForHouseholdAsync(Guid householdId)"
	method	=	"GET"
	url_template	=	"Room/forHousehold/{householdId}"
	description	=	"Returns a list of Rooms (sorted by name) for a given Household ID"
	template_parameter {
		name	=	"householdId"
		required	=	true
		type	=	"uuid"
		description	=	"The Household ID to return a list of Rooms for"
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
}



resource "azurerm_api_management_api_operation" "GetRoomAsync" {
	operation_id	=	"GetRoomAsync"
	api_name	=	azurerm_api_management_api.roombyroomsapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"GetRoomAsync(Guid roomId)"
	method	=	"GET"
	url_template	=	"Room/{roomId}"
	description	=	"Returns the Room object for roomId"
	template_parameter {
		name	=	"roomId"
		required	=	true
		type	=	"uuid"
		description	=	"Room ID for the Room to get"
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



resource "azurerm_api_management_api_operation" "UpdateRoom" {
	operation_id	=	"UpdateRoom"
	api_name	=	azurerm_api_management_api.roombyroomsapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"UpdateRoomAsync(Guid roomId, [FromBody] Room roomToUpdate)"
	method	=	"PUT"
	url_template	=	"Room/{roomId}"
	description	=	"Updates the Room roomId with the values from roomToUpdate if it exists; if roomId is omitted, a new Room will be created instead."
	template_parameter {
		name	=	"roomId"
		required	=	true
		type	=	"uuid"
		description	=	"Guid ID for the Room to update"
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
	response {
		status_code	=	200
		description	=	"Success"
		representation {
			content_type	=	"application/json"
		}
	}
}



resource "azurerm_api_management_api_operation" "DeleteRoomAsync" {
	operation_id	=	"DeleteRoomAsync"
	api_name	=	azurerm_api_management_api.roombyroomsapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"DeleteRoomAsync(Guid roomId)"
	method	=	"DELETE"
	url_template	=	"Room/{roomId}"
	description	=	"Deletes the Room with the given roomId"
	template_parameter {
		name	=	"roomId"
		required	=	true
		type	=	"uuid"
		description	=	"Room ID for the Room to delete"
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
		status_code	=	400
		description	=	"Bad Request"
		representation {
			content_type	=	"application/json"
		}
	}
	response {
		status_code	=	204
		description	=	"Success"
	}
}



resource "azurerm_api_management_api_operation" "CreateRoomAsync" {
	operation_id	=	"CreateRoomAsync"
	api_name	=	azurerm_api_management_api.roombyroomsapi_rev1.name
	api_management_name	=	data.azurerm_api_management.roombyapim.name
	resource_group_name	=	data.azurerm_api_management.roombyapim.resource_group_name
	display_name	=	"CreateRoomAsync(Room roomToCreate)"
	method	=	"POST"
	url_template	=	"Room"
	description	=	"Creates the provided roomToCreate"
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



