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
    
    $response->status = "ingelogd";
    $response->customMessage = "gefeliciteerd";
    $response->token = $token;
    die(json_encode($response));
}
else {
    $response->status = "error";
    $response->customMessage = "onjuist wachtwoord";
    die(json_encode($response));
}