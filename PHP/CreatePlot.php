<?php
//kijken op welke user het gebouwd moet worden
$stmt = $conn->prepare("SELECT * FROM users WHERE token = :token");
$stmt->bindParam(":token", $request->token);
$stmt->execute();
if ($stmt->rowCount() == 0) {
    $response->status = "invalid_token";
    $response->customMessage = "Token not found";
    die(json_encode($response));
}
$result = $stmt->fetch(PDO::FETCH_ASSOC);

$userid = $result['user_id'];

//Check of de tile al gebouwd is
$stmt = $conn->prepare("SELECT * FROM grid WHERE user_id = $userid AND PosX = :posX AND PosY = :posY");
$stmt->execute([
    ":posX" => $request->gridTileData->PosX,
    ":posY" => $request->gridTileData->PosY
]);
if ($stmt->rowCount() > 0) {
    $response->status = "plot_exists";
    $response->customMessage = "Plot already build";
    die(json_encode($response));
}

// eigenlijk check je hier ook of de speler het kan betalen

$stmt = $conn->prepare("INSERT INTO grid (PosX, PosY, Tile_Type, Last_Updated, user_id) VALUES (:PosX, :PosY, :Tile_Type, :Last_Updated, :user_id)");
if ($stmt->execute([
    ":PosX" => $request->gridTileData->PosX,
    ":PosY" => $request->gridTileData->PosY,
    ":Tile_Type" => $request->newPlotType,
    ":Last_Updated" => date("Y-m-d H:i:s"),
    ":user_id" => $userid
])) {
    $response->status = "succes";
    $response->customMessage = "The plot has been created";
    die(json_encode($response));
} else {
    $response->status = "error";
    $response->customMessage = "For some reason the plot was not inserted in the database";
    die (json_encode($response));
}
