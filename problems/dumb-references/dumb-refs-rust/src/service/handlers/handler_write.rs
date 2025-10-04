pub struct WriteHandler<'a> {
    buffer: &'a mut String,
}

impl<'a> WriteHandler<'a> {
    pub fn new(text: &'a mut String) -> Self {
        Self { buffer: text }
    }

    pub fn handle(&self) {
        println!("WriteHandler: {}", self.buffer);
    }
}
