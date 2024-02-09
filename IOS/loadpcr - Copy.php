<?php
$pcr = array();

$id = $_GET['id'];

$pcr["id"] = $id;

$sqlSelect = "select data from json_pcr where id = '" . $id . "'";


$conn = new mysqli($servername, $username, $password, $dbname);


// flat fields:
$pcr["field1"] = "value1";
$pcr["field2"] = "value2";

// make sections:
$demographics = array();
$demographics["first_name"] = "harry";
$demographics["last_name"] = "johnson";
$pcr["demographics"] = $demographics;

$return = $conn->query($sqlSelect);
$pcr =$return->fetch_assoc()["data"]
echo asdfsad;

?>
