grammar BogieLang;

/*
 * Parser Rules
 */
program				:		(functionDefinition)*;
functionDefinition	:		TYPE IDENTIFIER '(' ( ( TYPE IDENTIFIER )  (',' ( TYPE IDENTIFIER ) )* )? ')' '{' ( (varDeclaration | varDefinition | functionCall | functionReturn ) )* '}' ;
functionReturn		:		'return' expression;
varDeclaration		:		TYPE IDENTIFIER ( '=' expression )?;
varDefinition		:		IDENTIFIER '=' expression;
functionCall		:		IDENTIFIER '(' ( expression  (',' expression)* )? ')';
expression			:		(IDENTIFIER | literal | functionCall) (OPERATOR expression)?;
literal				:		REAL | INTEGER | BOOL | STRING;

/*
 * Lexer Rules
 */
WS					:		[ \t\n\r]+ -> skip ;
WS_S				:		' ' -> skip;
WS_T				:		'\t' -> skip;
WS_N				:		'\n' -> skip;
WS_R				:		'\r' -> skip;

REAL				:		[0-9]+'.'[0-9]+;
INTEGER				:		[0-9]+;
BOOL				:		('false' | 'true');
STRING				:		'"'[\u0000-\uFFFE]+'"';
NEWLINE				:		('\n'|'\r')+;
TYPE				:		('int' | 'real' | 'string' | 'bool' | 'void');
OPERATOR			:		('+' | '-' | '*' | '/' | '^' | '==' | '!=' | '<' | '>' | '<=' | '>='); 
IDENTIFIER			:		[A-Za-z]+[a-zA-Z0-9]*;