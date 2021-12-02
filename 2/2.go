package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

type instr struct {
	dir string
	num int
}

func main() {
	raw := strings.Split(os.Args[1], "\n")
	parsed := []instr{}
	for _, s := range raw {
		split := strings.Split(s, " ")
		i, _ := strconv.Atoi(split[1])
		parsed = append(parsed, instr{split[0], i})
	}

	depth, horiz, aim := 0, 0, 0

	for _, e := range parsed {
		if e.dir == "forward" {
			horiz += e.num
			depth += aim * e.num
		} else if e.dir == "up" {
			aim -= e.num
		} else if e.dir == "down" {
			aim += e.num
		}
	}

	fmt.Println(depth * horiz)
}
