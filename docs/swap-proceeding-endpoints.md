# Swap Proceeding Endpoints

## Close a swap proceeding

`POST /api/listing_item/close/swap_now?swapId={swapProceedingId}`

Authentication is required. The bearer token must belong to either participant in the proceeding.

This endpoint closes only the `SwappingProceeding`. It does not close or remove the related listing, so the listing can be managed independently.

### Request

```http
POST /api/listing_item/close/swap_now?swapId=SWAP_PROCEEDING_ID
Authorization: Bearer ACCESS_TOKEN
```

### Success response

```json
{
  "statusCode": 200,
  "displayMessage": "Success",
  "result": "Swap proceeding closed",
  "errorMessages": null
}
```

### Error responses

`404 Not Found` when the proceeding does not exist:

```json
{
  "statusCode": 404,
  "displayMessage": "Error",
  "result": null,
  "errorMessages": ["Swap proceeding not found"]
}
```

`403 Forbidden` when the authenticated user is not one of the proceeding participants:

```json
{
  "statusCode": 403,
  "displayMessage": "Error",
  "result": null,
  "errorMessages": ["You are not a participant in this swap proceeding"]
}
```
