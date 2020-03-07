grammar BogieLang;

/*
 * Parser Rules
 */

compileUnit
	:	EOF
	;

/*
 * Lexer Rules
 */

WS
	:	' ' -> channel(HIDDEN)
	;
