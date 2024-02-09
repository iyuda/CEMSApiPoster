<?php
$payload = file_get_contents('php://input');

$json = json_decode($payload);
echo json_last_error_msg();
echo '<pre>' . var_dump($json) . '</pre>';
echo '<hr/>';

$json = json_decode($payload, true);
echo json_last_error_msg();
echo '<pre>' . var_dump($json) . '</pre>';
echo '<hr/>';


$string = json_encode($json);
echo json_last_error_msg();

echo '<br/>' . $string;

?>