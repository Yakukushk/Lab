grammar LabCalculator;


/*
 * Parser Rules
 */

compileUnit : expression EOF;
expression :
	LPAREN expression RPAREN #ParenthesizedExpr
	|expression EXPONENT expression #ExponentialExpr
	| expression MOD expression  #ModExpression
    | expression DIV expression  #DivExpression
    | expression operatorToken=(MULTIPLY | DIVIDE) expression #MultiplicativeExpr
	| expression operatorToken=(ADD | SUBTRACT) expression #AdditiveExpr
	|operatorToken =(INCREMENT|DECREMENT) LPAREN expression RPAREN #IncDecExpr
	| MAX LPAREN expression ',' expression RPAREN #MaxExpreContext
    | MIN LPAREN expression ',' expression RPAREN #MinExpression
	| NUMBER #NumberExpr
	| IDENTIFIER #IdentifierExpr
	; 

/*
 * Lexer Rules
 */

NUMBER : INT ('.' INT)?; 
IDENTIFIER : [a-zA-Z]+[1-9][0-9]+;

INT : ('0'..'9')+;

EXPONENT : '^';
MULTIPLY : '*';
DIVIDE : '/';
SUBTRACT : '-';
ADD : '+';
LPAREN : '(';
RPAREN : ')';
MOD: 'mod';
INCREMENT: 'inc';
DECREMENT: 'dec';
DIV: 'div';
MAX: 'max';
MIN: 'min';

WS : [ \t\r\n] -> channel(HIDDEN);
