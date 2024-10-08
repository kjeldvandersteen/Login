<?php

$stmt = $conn->prepare("SELECT * FROM users WHERE token = :token");
$stmt = bindparam(":token", $request->token);
$stmt->execute();

$result = $stmt->fetch(PDO::FETCH_ASSOC);

$result['user_id'] = $userid;

$response->status = "error";
$response->customMessage = $userid;
die (json_encode($response));

