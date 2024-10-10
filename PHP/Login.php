<?php
 
$stmt = $conn->prepare("SELECT * FROM users WHERE email = :email");
$stmt->bindparam(":email", $request->email);
$stmt->execute();

$result = $stmt->fetch(PDO::FETCH_ASSOC);


if ($result == false) {
    $response->status = "error";
    $response->customMessage = "email bestaat nog niet";
    die (json_encode($response));
}

$hash = $result['password_hash'];

if (password_verify($request->password, $hash)) {

    $token = generateToken();
    $stmt = $conn->prepare("UPDATE users SET token = :token WHERE user_id = :id");
    $stmt->bindparam(":token", $token);
    $stmt->bindparam(":id", $result['user_id']);
    $stmt->execute();
    
    
}
else {
    $response->status = "error";
    $response->customMessage = "onjuist wachtwoord";
    die(json_encode($response));
}


$stmt = $conn->prepare("SELECT * FROM users WHERE token = :token");
$stmt->bindParam(":token", $token);
$stmt->execute();
if ($stmt->rowCount() == 0) {
    $response->status = "invalid_token";
    $response->customMessage = "Token not found";
    die(json_encode($response));
}

$result = $stmt->fetch(PDO::FETCH_ASSOC);
$userid = $result['user_id'];

$response->status = "ingelogd";
$response->customMessage = "gefeliciteerd";
$response->token = $userid;
die(json_encode($response));