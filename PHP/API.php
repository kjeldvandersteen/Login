<?php 
$names = ["Kees", "Henk", "John", "Mien"];
$villager = new stdClass();
$villager->name = $names[rand(0, 3)];
$villager->age = rand(18, 60);
$villager->craftSkill = rand(0, 100);
$villager->fightSkill = rand(0, 100);

echo(json_encode($villager));
?>
