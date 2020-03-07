grammar BogieLang;

/*
 * Parser Rules
 */
literal			:		REAL | INTEGER | BOOL | STRING;

/*
 * Lexer Rules
 */

REAL			:		[0-9]+'.'[0-9]+;
INTEGER			:		[0-9]+;
BOOL			:		('false' | 'true');
STRING			:		'"'[a-zA-Z0-9]+'"';
NEWLINE			:		('\n'|'\r')+;
TYPE			:		('int' | 'real' | 'string' | 'bool' | 'void');
OPERATOR		:		('+' | '-' | '*' | '/' | '^' | '==' | '!=' | '<' | '>' | '<=' | '>='); 
IDENTIFIER		:		[A-Za-z]+[a-zA-Z0-9]*;