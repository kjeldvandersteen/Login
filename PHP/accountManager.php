<?php

// Het response object zullen we altijd outputten in JSON naar de client
$response = new stdClass();

// Check of 'data' bestaat in the POST array
if (!isset($_POST['data'])) {
    $response->status = "invalidPostData";
    $response->customMessage = "The key 'data' does not exist in the POST array.";
    die(json_encode($response));
}

// Probeer de JSON data te decoden in een PHP object
$request = json_decode($_POST['data']);

if (json_last_error() !== JSON_ERROR_NONE) {
    // JSON decoden is mislukt, dus sturen we via JSON de error terug
    $response->status = "invalidJson";
    $response->customMessage = "The provided JSON data could not be decoded: " . json_last_error_msg();
    die(json_encode($response));
}

// Nadat alle data goed is aangeleverd proberen we een connectie met de database te maken
$conn = getDatabaseConnection();

if (is_array($conn) && isset($conn['error']) && $conn['error']) {
    // We checken of er een error was, in dat geval bestaat 'error' in 'conn'
    // (zie de getDatabaseConnection() functie beneden)
    $response->status = "databaseConnectionError";
    $response->customMessage = $conn['message'];
    die(json_encode($response));
}

// Check of er wel een 'action' key bestaat in de request
if (!isset($request->action) || empty($request->action)) {
    $response->status = "invalidAction";
    $response->customMessage = "The field 'action' is missing or empty in the request.";
    die(json_encode($response));
}

// Mocht het script op dit punt zijn gekomen, dan betekent dit dat je JSON valide is
// en een database connectie is gemaakt, nu zal de juist functie uitgevoerd moeten worden
// aan de hand van welke 'action' gedaan moet worden

switch ($request->action) {
    case "createAccount":
        include 'CreateAccount.php';
        break;
    case "loginAccount":
        include 'Login.php';
        generateToken();
        break;
    case "logoutRequest":
        include 'Logout.php';
        break;
    case "CreatePlot":
        include 'CreatePlot.php';
        break;
    default:
        $response->status = "noValidAction";
        $response->customMessage = "The action does not exist on the server.";
        die(json_encode($response));
}

function getDatabaseConnection() {
    $dbHost = '127.0.0.1';
    $dbName = 'example';
    $dbUser = 'root';
    $dbPass = '';

    try {
        // Probeer een connectie te maken met de database
        $pdo = new PDO("mysql:host=$dbHost;dbname=$dbName", $dbUser, $dbPass);
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        return $pdo; // Return de PDO instance als het verbinden lukt
    } catch (PDOException $e) {
        // Return de error informatie in een array als het verbinden mislukt
        return [
            'error' => true,
            'message' => $e->getMessage()
        ];
    }
}

// Gebruik deze functie om een token te generaten wanneer een gebruiker succesvol inlogt, 64 is lang genoeg
function generateToken($length = 64) {
    return bin2hex(random_bytes($length / 2));
}
