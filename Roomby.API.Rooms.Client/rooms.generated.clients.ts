/* tslint:disable */
/* eslint-disable */
//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.10.4.0 (NJsonSchema v10.3.7.0 (Newtonsoft.Json v12.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------
// ReSharper disable InconsistentNaming

import * as moment from 'moment';

export class AuthClient {
    constructor(private roomsApiUrl: string) {
    }

    getBaseUrl(defaultUrl: string, requestedUrl?: string) {
        return requestedUrl ? requestedUrl : this.roomsApiUrl;
    }

    transformHttpRequestOptions(options: RequestInit): Promise<RequestInit> {
        return Promise.resolve(options);
    }
}

export class AuthorizedApiBase {
    protected constructor(private authClient: AuthClient) {

    }

    getBaseUrl(defaultUrl: string, baseUrl: string) {
        return this.authClient ? this.authClient.getBaseUrl(defaultUrl) : baseUrl;
    }

    transformOptions(options: RequestInit): Promise<RequestInit> {
        return this.authClient ? this.authClient.transformHttpRequestOptions(options) : Promise.resolve(options);
    }
}

export interface IRoomClient {
    /**
     * GetRoomsForHouseholdAsync(Guid householdId)
     * @param householdId The Household ID to return a list of Rooms for
     * @return A List of Room objects for the given Household ID
     */
    getRoomsForHousehold(householdId: string): Promise<Room[]>;
    /**
     * GetRoomAsync(Guid roomId)
     * @param roomId Room ID for the Room to get
     * @return Room object with id roomId
     */
    getRoom(roomId: string): Promise<Room>;
    /**
     * UpdateRoomAsync(Guid roomId, [FromBody] Room roomToUpdate)
     * @param roomId Guid ID for the Room to update
     * @param roomToUpdate Room object with the request changes filled in
     * @return The updated room object, or a newly created one if roomId is omitted
     */
    updateRoom(roomId: string, roomToUpdate: Room): Promise<Room>;
    /**
     * DeleteRoomAsync(Guid roomId)
     * @param roomId Room ID for the Room to delete
     * @return NoContent if successfully deleted; if ID can't be found or is not provided, BadRequest is returned.
     */
    deleteRoom(roomId: string): Promise<void>;
    /**
     * CreateRoomAsync(Room roomToCreate)
     * @param roomToCreate A Room object. See CreateRoomValidator for validation information
     * @return The created Room object
     */
    createRoom(roomToCreate: Room): Promise<Room>;
}

export class RoomClient extends AuthorizedApiBase implements IRoomClient {
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(configuration: AuthClient, baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
        super(configuration);
        this.http = http ? http : <any>window;
        this.baseUrl = this.getBaseUrl("", baseUrl);
    }

    /**
     * GetRoomsForHouseholdAsync(Guid householdId)
     * @param householdId The Household ID to return a list of Rooms for
     * @return A List of Room objects for the given Household ID
     */
    getRoomsForHousehold(householdId: string): Promise<Room[]> {
        let url_ = this.baseUrl + "/api/v1/Room/forHousehold/{householdId}";
        if (householdId === undefined || householdId === null)
            throw new Error("The parameter 'householdId' must be defined.");
        url_ = url_.replace("{householdId}", encodeURIComponent("" + householdId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processGetRoomsForHousehold(_response);
        });
    }

    protected processGetRoomsForHousehold(response: Response): Promise<Room[]> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result401 = ProblemDetails.fromJS(resultData401, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result401);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            let result403: any = null;
            let resultData403 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result403 = ProblemDetails.fromJS(resultData403, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result403);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            if (Array.isArray(resultData200)) {
                result200 = [] as any;
                for (let item of resultData200)
                    result200!.push(Room.fromJS(item, _mappings));
            }
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Room[]>(<any>null);
    }

    /**
     * GetRoomAsync(Guid roomId)
     * @param roomId Room ID for the Room to get
     * @return Room object with id roomId
     */
    getRoom(roomId: string): Promise<Room> {
        let url_ = this.baseUrl + "/api/v1/Room/{roomId}";
        if (roomId === undefined || roomId === null)
            throw new Error("The parameter 'roomId' must be defined.");
        url_ = url_.replace("{roomId}", encodeURIComponent("" + roomId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processGetRoom(_response);
        });
    }

    protected processGetRoom(response: Response): Promise<Room> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result401 = ProblemDetails.fromJS(resultData401, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result401);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            let result403: any = null;
            let resultData403 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result403 = ProblemDetails.fromJS(resultData403, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result403);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result200 = Room.fromJS(resultData200, _mappings);
            return result200;
            });
        } else if (status === 404) {
            return response.text().then((_responseText) => {
            let result404: any = null;
            let resultData404 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result404 = ProblemDetails.fromJS(resultData404, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result404);
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Room>(<any>null);
    }

    /**
     * UpdateRoomAsync(Guid roomId, [FromBody] Room roomToUpdate)
     * @param roomId Guid ID for the Room to update
     * @param roomToUpdate Room object with the request changes filled in
     * @return The updated room object, or a newly created one if roomId is omitted
     */
    updateRoom(roomId: string, roomToUpdate: Room): Promise<Room> {
        let url_ = this.baseUrl + "/api/v1/Room/{roomId}";
        if (roomId === undefined || roomId === null)
            throw new Error("The parameter 'roomId' must be defined.");
        url_ = url_.replace("{roomId}", encodeURIComponent("" + roomId));
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(roomToUpdate);

        let options_ = <RequestInit>{
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processUpdateRoom(_response);
        });
    }

    protected processUpdateRoom(response: Response): Promise<Room> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result401 = ProblemDetails.fromJS(resultData401, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result401);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            let result403: any = null;
            let resultData403 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result403 = ProblemDetails.fromJS(resultData403, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result403);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result400 = ProblemDetails.fromJS(resultData400, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 201) {
            return response.text().then((_responseText) => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result201 = Room.fromJS(resultData201, _mappings);
            return result201;
            });
        } else if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result200 = Room.fromJS(resultData200, _mappings);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Room>(<any>null);
    }

    /**
     * DeleteRoomAsync(Guid roomId)
     * @param roomId Room ID for the Room to delete
     * @return NoContent if successfully deleted; if ID can't be found or is not provided, BadRequest is returned.
     */
    deleteRoom(roomId: string): Promise<void> {
        let url_ = this.baseUrl + "/api/v1/Room/{roomId}";
        if (roomId === undefined || roomId === null)
            throw new Error("The parameter 'roomId' must be defined.");
        url_ = url_.replace("{roomId}", encodeURIComponent("" + roomId));
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "DELETE",
            headers: {
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processDeleteRoom(_response);
        });
    }

    protected processDeleteRoom(response: Response): Promise<void> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result401 = ProblemDetails.fromJS(resultData401, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result401);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            let result403: any = null;
            let resultData403 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result403 = ProblemDetails.fromJS(resultData403, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result403);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result400 = ProblemDetails.fromJS(resultData400, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 204) {
            return response.text().then((_responseText) => {
            return;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<void>(<any>null);
    }

    /**
     * CreateRoomAsync(Room roomToCreate)
     * @param roomToCreate A Room object. See CreateRoomValidator for validation information
     * @return The created Room object
     */
    createRoom(roomToCreate: Room): Promise<Room> {
        let url_ = this.baseUrl + "/api/v1/Room";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(roomToCreate);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.transformOptions(options_).then(transformedOptions_ => {
            return this.http.fetch(url_, transformedOptions_);
        }).then((_response: Response) => {
            return this.processCreateRoom(_response);
        });
    }

    protected processCreateRoom(response: Response): Promise<Room> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        let _mappings: { source: any, target: any }[] = [];
        if (status === 401) {
            return response.text().then((_responseText) => {
            let result401: any = null;
            let resultData401 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result401 = ProblemDetails.fromJS(resultData401, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result401);
            });
        } else if (status === 403) {
            return response.text().then((_responseText) => {
            let result403: any = null;
            let resultData403 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result403 = ProblemDetails.fromJS(resultData403, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result403);
            });
        } else if (status === 500) {
            return response.text().then((_responseText) => {
            return throwException("A server side error occurred.", status, _responseText, _headers);
            });
        } else if (status === 400) {
            return response.text().then((_responseText) => {
            let result400: any = null;
            let resultData400 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result400 = ProblemDetails.fromJS(resultData400, _mappings);
            return throwException("A server side error occurred.", status, _responseText, _headers, result400);
            });
        } else if (status === 201) {
            return response.text().then((_responseText) => {
            let result201: any = null;
            let resultData201 = _responseText === "" ? null : jsonParse(_responseText, this.jsonParseReviver);
            result201 = Room.fromJS(resultData201, _mappings);
            return result201;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<Room>(<any>null);
    }
}

export class ProblemDetails implements IProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    extensions?: { [key: string]: any; } | undefined;

    constructor(data?: IProblemDetails) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.type = _data["type"];
            this.title = _data["title"];
            this.status = _data["status"];
            this.detail = _data["detail"];
            this.instance = _data["instance"];
            if (_data["extensions"]) {
                this.extensions = {} as any;
                for (let key in _data["extensions"]) {
                    if (_data["extensions"].hasOwnProperty(key))
                        this.extensions![key] = _data["extensions"][key];
                }
            }
        }
    }

    static fromJS(data: any, _mappings?: any): ProblemDetails | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<ProblemDetails>(data, _mappings, ProblemDetails);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["type"] = this.type;
        data["title"] = this.title;
        data["status"] = this.status;
        data["detail"] = this.detail;
        data["instance"] = this.instance;
        if (this.extensions) {
            data["extensions"] = {};
            for (let key in this.extensions) {
                if (this.extensions.hasOwnProperty(key))
                    data["extensions"][key] = this.extensions[key];
            }
        }
        return data; 
    }
}

export interface IProblemDetails {
    type?: string | undefined;
    title?: string | undefined;
    status?: number | undefined;
    detail?: string | undefined;
    instance?: string | undefined;
    extensions?: { [key: string]: any; } | undefined;
}

export class Room implements IRoom {
    id?: string;
    name?: string | undefined;
    createdAt?: moment.Moment;
    modifiedAt?: moment.Moment;
    householdId?: string;
    icon?: string | undefined;
    purchaseTotal?: number;
    boughtTotal?: number;
    household?: Household | undefined;

    constructor(data?: IRoom) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"];
            this.name = _data["name"];
            this.createdAt = _data["createdAt"] ? moment.parseZone(_data["createdAt"].toString()) : <any>undefined;
            this.modifiedAt = _data["modifiedAt"] ? moment.parseZone(_data["modifiedAt"].toString()) : <any>undefined;
            this.householdId = _data["householdId"];
            this.icon = _data["icon"];
            this.purchaseTotal = _data["purchaseTotal"];
            this.boughtTotal = _data["boughtTotal"];
            this.household = _data["household"] ? Household.fromJS(_data["household"], _mappings) : <any>undefined;
        }
    }

    static fromJS(data: any, _mappings?: any): Room | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<Room>(data, _mappings, Room);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString(true) : <any>undefined;
        data["modifiedAt"] = this.modifiedAt ? this.modifiedAt.toISOString(true) : <any>undefined;
        data["householdId"] = this.householdId;
        data["icon"] = this.icon;
        data["purchaseTotal"] = this.purchaseTotal;
        data["boughtTotal"] = this.boughtTotal;
        data["household"] = this.household ? this.household.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IRoom {
    id?: string;
    name?: string | undefined;
    createdAt?: moment.Moment;
    modifiedAt?: moment.Moment;
    householdId?: string;
    icon?: string | undefined;
    purchaseTotal?: number;
    boughtTotal?: number;
    household?: Household | undefined;
}

export class Household implements IHousehold {
    id?: string;
    name?: string | undefined;
    headOfHouseholdId?: string;
    createdAt?: moment.Moment;
    modifiedAt?: moment.Moment;
    headOfHousehold?: User | undefined;

    constructor(data?: IHousehold) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"];
            this.name = _data["name"];
            this.headOfHouseholdId = _data["headOfHouseholdId"];
            this.createdAt = _data["createdAt"] ? moment.parseZone(_data["createdAt"].toString()) : <any>undefined;
            this.modifiedAt = _data["modifiedAt"] ? moment.parseZone(_data["modifiedAt"].toString()) : <any>undefined;
            this.headOfHousehold = _data["headOfHousehold"] ? User.fromJS(_data["headOfHousehold"], _mappings) : <any>undefined;
        }
    }

    static fromJS(data: any, _mappings?: any): Household | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<Household>(data, _mappings, Household);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["headOfHouseholdId"] = this.headOfHouseholdId;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString(true) : <any>undefined;
        data["modifiedAt"] = this.modifiedAt ? this.modifiedAt.toISOString(true) : <any>undefined;
        data["headOfHousehold"] = this.headOfHousehold ? this.headOfHousehold.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IHousehold {
    id?: string;
    name?: string | undefined;
    headOfHouseholdId?: string;
    createdAt?: moment.Moment;
    modifiedAt?: moment.Moment;
    headOfHousehold?: User | undefined;
}

export class User implements IUser {
    id?: string;
    householdId?: string;
    createdAt?: moment.Moment;
    modifiedAt?: moment.Moment;
    fullName?: string | undefined;
    email?: string | undefined;
    identity?: string | undefined;
    provider?: string | undefined;
    subscriptionId?: string | undefined;
    household?: Household | undefined;

    constructor(data?: IUser) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any, _mappings?: any) {
        if (_data) {
            this.id = _data["id"];
            this.householdId = _data["householdId"];
            this.createdAt = _data["createdAt"] ? moment.parseZone(_data["createdAt"].toString()) : <any>undefined;
            this.modifiedAt = _data["modifiedAt"] ? moment.parseZone(_data["modifiedAt"].toString()) : <any>undefined;
            this.fullName = _data["fullName"];
            this.email = _data["email"];
            this.identity = _data["identity"];
            this.provider = _data["provider"];
            this.subscriptionId = _data["subscriptionId"];
            this.household = _data["household"] ? Household.fromJS(_data["household"], _mappings) : <any>undefined;
        }
    }

    static fromJS(data: any, _mappings?: any): User | null {
        data = typeof data === 'object' ? data : {};
        return createInstance<User>(data, _mappings, User);
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["householdId"] = this.householdId;
        data["createdAt"] = this.createdAt ? this.createdAt.toISOString(true) : <any>undefined;
        data["modifiedAt"] = this.modifiedAt ? this.modifiedAt.toISOString(true) : <any>undefined;
        data["fullName"] = this.fullName;
        data["email"] = this.email;
        data["identity"] = this.identity;
        data["provider"] = this.provider;
        data["subscriptionId"] = this.subscriptionId;
        data["household"] = this.household ? this.household.toJSON() : <any>undefined;
        return data; 
    }
}

export interface IUser {
    id?: string;
    householdId?: string;
    createdAt?: moment.Moment;
    modifiedAt?: moment.Moment;
    fullName?: string | undefined;
    email?: string | undefined;
    identity?: string | undefined;
    provider?: string | undefined;
    subscriptionId?: string | undefined;
    household?: Household | undefined;
}

function jsonParse(json: any, reviver?: any) {
    json = JSON.parse(json, reviver);

    var byid: any = {};
    var refs: any = [];
    json = (function recurse(obj: any, prop?: any, parent?: any) {
        if (typeof obj !== 'object' || !obj)
            return obj;
        
        if ("$ref" in obj) {
            let ref = obj.$ref;
            if (ref in byid)
                return byid[ref];
            refs.push([parent, prop, ref]);
            return undefined;
        } else if ("$id" in obj) {
            let id = obj.$id;
            delete obj.$id;
            if ("$values" in obj)
                obj = obj.$values;
            byid[id] = obj;
        }
        
        if (Array.isArray(obj)) {
            obj = obj.map((v, i) => recurse(v, i, obj));
        } else {
            for (var p in obj) {
                if (obj.hasOwnProperty(p) && obj[p] && typeof obj[p] === 'object')
                    obj[p] = recurse(obj[p], p, obj);
            }
        }

        return obj;
    })(json);

    for (let i = 0; i < refs.length; i++) {
        const ref = refs[i];
        ref[0][ref[1]] = byid[ref[2]];
    }

    return json;
}

function createInstance<T>(data: any, mappings: any, type: any): T | null {
  if (!mappings)
    mappings = [];
  if (!data)
    return null;

  const mappingIndexName = "__mappingIndex";
  if (data[mappingIndexName])
    return <T>mappings[data[mappingIndexName]].target;

  data[mappingIndexName] = mappings.length;

  let result: any = new type();
  mappings.push({ source: data, target: result });
  result.init(data, mappings);
  return result;
}

export class ApiException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
    if (result !== null && result !== undefined)
        throw result;
    else
        throw new ApiException(message, status, response, headers, null);
}

export class OidcClientSettings {
    static create(settings: {
        serverUrl: string, stsServerUrl: string, clientId: string, customerAlias: string,
        customerId: string, scope: string, redirectServerUrl?: string, logoutServerUrl?: string
    }) {

        return {
            client_id: settings.clientId,
            scope: settings.scope,
            authority: settings.stsServerUrl,
            response_type: "id_token token",
            filterProtocolClaims: true,
            loadUserInfo: true,
            redirect_uri: settings.redirectServerUrl ? settings.redirectServerUrl : settings.serverUrl + '/auth-callback',
            post_logout_redirect_uri: settings.logoutServerUrl ? settings.logoutServerUrl : settings.serverUrl,
            acr_values: 'tenant:{"id":"' +
                settings.customerId + '","alias":"' +
                settings.customerAlias + '"}'
        }
    }
}

export class AccessTokenAuthClient extends AuthClient {
    constructor(roomsApiUrl: string, private accessToken: string) {
        super(roomsApiUrl);
    }

    transformHttpRequestOptions(options: RequestInit): Promise<RequestInit> {
        if (options.headers && this.accessToken) {
            options.headers['Authorization'] = 'Bearer ' + this.accessToken;
        }

        return super.transformHttpRequestOptions(options);
    }
}