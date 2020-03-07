grammar BogieLang;

/*
 * Parser Rules
 */
program				:		(functionDefinition)*;
functionDefinition	:		TYPE ' ' IDENTIFIER'(' ( ( TYPE ' ' IDENTIFIER )  ( ',' ( TYPE ' ' IDENTIFIER ) )* )? ')' '{' ( (varDeclaration | varDefinition | functionCall | functionReturn ) NEWLINE )* '}';
functionReturn		:		'return' ' ' expression;
varDeclaration		:		TYPE ' ' IDENTIFIER ( '=' expression )?;
varDefinition		:		IDENTIFIER '=' expression;
functionCall		:		IDENTIFIER '(' ( expression  (',' expression)* )? ')';
expression			:		IDENTIFIER | literal | functionCall;
literal				:		REAL | INTEGER | BOOL | STRING;

/*
 * Lexer Rules
 */

REAL				:		[0-9]+'.'[0-9]+;
INTEGER				:		[0-9]+;
BOOL				:		('false' | 'true');
STRING				:		'"'[a-zA-Z0-9]+'"';
NEWLINE				:		('\n'|'\r')+;
TYPE				:		('int' | 'real' | 'string' | 'bool' | 'void');
OPERATOR			:		('+' | '-' | '*' | '/' | '^' | '==' | '!=' | '<' | '>' | '<=' | '>='); 
IDENTIFIER			:		[A-Za-z]+[a-zA-Z0-9]*;