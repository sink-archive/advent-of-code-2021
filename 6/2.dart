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
  var fish = List.filled(9, 0);
  // init the array
  for (final f in rawFish) fish[f]++;

  // for each day
  for (var i = 0; i < 256; i++) {
    final fishToReset = fish[0];
    for (var i = 1; i <= 8; i++) fish[i - 1] = fish[i];
    fish[8] = fishToReset;
    fish[6] += fishToReset;
  }

  print(fish.reduce((c, n) => c + n));
}
