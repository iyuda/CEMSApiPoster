<?php
$cad = array();

$id = $_GET['id'];

$cad["id"] = $id;

// flat fields:
$cad["field1"] = "value1";
$cad["field2"] = "value2";

// make sections:
$demographics = array();
$demographics["first_name"] = "harry";
$demographics["last_name"] = "johnson";
$cad["demographics"] = $demographics;


echo json_encode($cad);

?>
