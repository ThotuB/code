module BCDadd(
    input[3:0] x, y,
    input ci,
    output[3:0] z,
    output co
);
    wire c01, c12, c23, c3o;
    wire z1s, z2s, z3s;

    FAC fac0(.x(x[0]), .y(y[0]), .ci(ci) , .co(c01), .z(z[0]));
    FAC fac1(.x(x[1]), .y(y[1]), .ci(c01), .co(c12), .z(z1s));
    FAC fac2(.x(x[2]), .y(y[2]), .ci(c12), .co(c23), .z(z2s));
    FAC fac3(.x(x[3]), .y(y[3]), .ci(c23), .co(c3o), .z(z3s));

    assign co = c3o | (z3s & z2s) | (z3s & z1s);

    wire cu12, cu23;

    HAC hac11(.x(z1s), .y(co), .c(cu12), .z(z[1]));
    FAC fac22(.x(z2s), .y(co), .ci(cu12), .co(cu23), .z(z[2]));

    assign z[3] = z3s ^ cu23;

endmodule

module BCDadd_tb;
    reg[3:0] x, y;
    reg ci;
    wire[3:0] z;
    wire co;

    BCDadd inst(.x(x), .y(y), .ci(ci), .z(z), .co(co));

    initial begin
        $display("x\ty\tci\t|\tco\tz");
        $monitor("%d\t%d\t%b\t|\t%b\t%d", x, y, ci, co, z);
    end

    integer i, j;
    initial begin
        {x, y} = 0;
        for ( i = 1 ; i < 10 ; i = i+1) begin
            #10 x = i; y = 0;
            for ( j = 1 ; j < 10 ; j = j+1) begin
                #10 y = j;
            end
        end
    end
endmodule