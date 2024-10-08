<?php

$stmt = $conn->prepare("UPDATE users SET token=null WHERE token = :token ");
$stmt->bindparam(":token", $request->token);
$stmt->execute();

$response->status = "logout";
$response->customMessage = "token is gedelete";
die(json_encode($response));
