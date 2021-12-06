import 'dart:convert';
import 'dart:io';

import 'dart:math';

String readAllStdin() {
  var working = "";
  while (true) {
    final line = stdin.readLineSync(encoding: utf8);
    if (line == null) {
      return working.substring(0, working.length - 1);
    }

    working += line + "\n";
  }
}

void main() {
  final rawFish = readAllStdin().split(",").map(int.parse).toList();

  // setup a list with one slot for each counter
  var fish = List.filled(10, 0);
  // init the array
  for (final f in rawFish) fish[f + 1]++;

  // for each day
  for (var i = 0; i < 256; i++) {
    for (var i = 1; i <= 9; i++) fish[i - 1] = fish[i];
    final fishToReset = fish[0];
    fish[9] = fishToReset;
    fish[7] += fishToReset;
  }

  fish[0] = 0;

  print(fish.reduce((c, n) => c + n));
}
